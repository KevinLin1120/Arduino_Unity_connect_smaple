using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;
using System.Threading;
using UnityEngine.UI;

public class arduino_serial_port_test : MonoBehaviour
{
    public GameObject testCube;
    SerialPort serialPort;
    
    Thread readThread;

    bool isNewMessage;
    string readMessage;
    public Toggle ledToggle;

    void Start()
    {
        serialPort = new SerialPort("/dev/cu.usbmodem11301", 9600);
        serialPort.ReadTimeout = 10;
        try
        {
            // Open the serial serialPort
            serialPort.Open();
            readThread = new Thread(new ThreadStart(readData));
            readThread.Start();
            Debug.Log("SerialPort start to connect.");
        }
        catch
        {
            Debug.Log("SerialPort connct error.");
        }
        
        Debug.Log("Open port success");

        // 清空串口緩衝區
        serialPort.DiscardInBuffer();

        // Led toggle function
        ledToggle.onValueChanged.AddListener(OnToggleValueChanged);
    }

    void Update()
    {
        if (isNewMessage)
        {
            if (readMessage.Trim() == "1")
            {
                testCube.SetActive(true);
            }
            else
            {
                testCube.SetActive(false);
            }
        }
        isNewMessage = false;
    }

    void readData()
    {
        while (serialPort.IsOpen)
        {
            try
            {
                readMessage = serialPort.ReadLine();
                isNewMessage = true;
            }
            catch(System.Exception e)
            {
                if(e.Message != "The operation has timed out.")
                    Debug.Log(e.Message);
            }
        }
    }

    void writeData(string message)
    {
        Debug.Log(message);
        try
        {
            serialPort.WriteLine(message);
        }
        catch (System.Exception e)
        {
            Debug.LogWarning(e);
        }
    }

    void OnApplicationQuit()
    {
        if (serialPort != null && serialPort.IsOpen)
        {
            //Open the serial serialPort
            serialPort.Close(); 
        }
    }

    /// Toggle the Led
    void OnToggleValueChanged(bool val)
    {
        string ledStatus = val == true ? "1" : "0";
        writeData(ledStatus);
    }
}
