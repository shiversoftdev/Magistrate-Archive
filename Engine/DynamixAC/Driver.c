#include <ntddk.h>
#include <wdf.h>

DRIVER_INITIALIZE DriverEntry;
EVT_WDF_DRIVER_DEVICE_ADD AntiCheatEvtDeviceAdd;

//DriverEntry is like the main method
NTSTATUS
DriverEntry(
	_In_ PDRIVER_OBJECT     DriverObject,
	_In_ PUNICODE_STRING    RegistryPath
)
{
	NTSTATUS status = STATUS_SUCCESS;



	return status;
}

//https://docs.microsoft.com/en-us/windows-hardware/drivers/gettingstarted/writing-a-very-small-kmdf--driver
//https://docs.microsoft.com/en-us/windows-hardware/drivers/gettingstarted/device-nodes-and-device-stacks
//https://docs.microsoft.com/en-us/windows-hardware/drivers/samples/file-system-driver-samples
//https://docs.microsoft.com/en-us/windows-hardware/drivers/install/system-defined-device-setup-classes-available-to-vendors
//https://docs.microsoft.com/en-us/windows/desktop/api/winsvc/nf-winsvc-createservicea