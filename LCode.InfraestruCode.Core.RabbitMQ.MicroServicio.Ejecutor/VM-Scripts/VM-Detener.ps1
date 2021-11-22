$ConexionServer = Connect-VIServer $Servidor -user $Usuario -password $Contrasenia
$VM = Get-VM -Name $NombreVM
$VM.PowerState
$AccionVM = Stop-VM -VM $VM -Confirm:$false
$VM = Get-VM -Name $NombreVM
$VM.PowerState