using HidSharp;
using System;
using System.Diagnostics;

public sealed class FatsCoHidLightController : IDisposable
{
    private readonly HidDevice _device;
    private HidStream _stream;
    private bool _isOpen;

    private const byte HeaderReportId = 0x01;
    private const byte HeaderMagic = 0x5A;

    private const byte BlueLeds = 0x20;
    private const byte GreenLeds = 0x40;
    private const byte YellowLeds = 0x60;
    private const byte RedLeds = 0x80;

    private const byte StrobeSlow = 0x03;
    private const byte StrobeMedium = 0x04;
    private const byte StrobeFast = 0x05;
    private const byte StrobeFastest = 0x06;
    private const byte StrobeOff = 0x07;

    private const byte None = 0x00;
    private const byte All = 0xFF;

    private byte _currentRedMask;
    private byte _currentBlueMask;
    private byte _currentGreenMask;
    private byte _currentYellowMask;
    private bool _hasSentState;

    public FatsCoHidLightController(HidDevice device)
    {
        _device = device ?? throw new ArgumentNullException(nameof(device));
    }

    public bool Open()
    {
        if (_device.TryOpen(out _stream))
        {
            _stream.ReadTimeout = 0;
            _stream.WriteTimeout = 100;

            Debug.WriteLine("Opened FatsCo HID device.");
            Debug.WriteLine($"Output report length: {_device.GetMaxOutputReportLength()}");
            Debug.WriteLine($"Feature report length: {_device.GetMaxFeatureReportLength()}");

            _isOpen = true;

            AllOff();

            return true;
        }

        Debug.WriteLine("Failed to open FatsCo HID device.");
        return false;
    }

    private void SendBytesToLightKit(byte command, byte parameter)
    {
        if (!_isOpen || _stream == null)
            return;

        byte[] report =
        {
            HeaderReportId,
            HeaderMagic,
            parameter,
            command
        };

        try
        {
            _stream.Write(report);
        }
        catch (Exception ex)
        {
            Debug.WriteLine("FatsCo write failed:");
            Debug.WriteLine(ex.ToString());
        }
    }

    public void SetLedMasks(byte redMask, byte blueMask, byte greenMask, byte yellowMask)
    {
        if (!_hasSentState || _currentRedMask != redMask)
        {
            SendBytesToLightKit(RedLeds, redMask);
            _currentRedMask = redMask;
        }

        if (!_hasSentState || _currentBlueMask != blueMask)
        {
            SendBytesToLightKit(BlueLeds, blueMask);
            _currentBlueMask = blueMask;
        }

        if (!_hasSentState || _currentGreenMask != greenMask)
        {
            SendBytesToLightKit(GreenLeds, greenMask);
            _currentGreenMask = greenMask;
        }

        if (!_hasSentState || _currentYellowMask != yellowMask)
        {
            SendBytesToLightKit(YellowLeds, yellowMask);
            _currentYellowMask = yellowMask;
        }

        _hasSentState = true;
    }

    public void AllOff()
    {
        SendBytesToLightKit(BlueLeds, None);
        SendBytesToLightKit(GreenLeds, None);
        SendBytesToLightKit(YellowLeds, None);
        SendBytesToLightKit(RedLeds, None);
        SendBytesToLightKit(StrobeOff, None);

        _currentBlueMask = None;
        _currentGreenMask = None;
        _currentYellowMask = None;
        _currentRedMask = None;
        _hasSentState = true;
    }

    public void AllOn()
    {
        SetLedMasks(All, All, All, All);
    }

    public void StrobeOnSlowest()
    {
        SendBytesToLightKit(StrobeSlow, None);
    }

    public void StrobeOnMedium()
    {
        SendBytesToLightKit(StrobeMedium, None);
    }

    public void StrobeOnFast()
    {
        SendBytesToLightKit(StrobeFast, None);
    }

    public void StrobeOnFastest()
    {
        SendBytesToLightKit(StrobeFastest, None);
    }

    public void TurnOffStrobe()
    {
        SendBytesToLightKit(StrobeOff, None);
    }

    public void Dispose()
    {
        try
        {
            AllOff();
        }
        catch
        {
            // Ignore shutdown errors.
        }

        _stream?.Dispose();
        _stream = null;
        _isOpen = false;
    }
}