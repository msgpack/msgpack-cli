<#
.SYNOPSIS
Make test certificate for UWP unit test programs.
.PARAMETER FilePath
Specify output file path. Default is "./testcert.pfx"
.DESCRIPTION
This script makes self-signed certificates for code signing with CN=MsgPack.Cli.UnitTest, RSA256, SHA256 with maximum expiry date.
Note that the pfx file has empty password.
#>
#requires -Version 5.1

using namespace System.IO
using namespace System.Security.Cryptography
using namespace System.Security.Cryptography.X509Certificates

[CmdletBinding(PositionalBinding = $false)]
param(
    [ValidateNotNullOrEmpty()][string]$FilePath = "./testcert.pfx"
)

Set-StrictMode -Version 5.1

[string]$subject = "CN=MsgPack.Cli.UnitTest"
[RSA]$password = [RSA]::Create(2048)
[HashAlgorithmName]$hashAlgorithm = [HashAlgorithmName]::SHA256
[RSASignaturePadding]$padding = [RSASignaturePadding]::Pkcs1
[DateTimeOffset]$notBefore = [System.DateTimeOffset]::new(2019, 10, 1, 0, 0, 0, 0)
# Some implementation has year 2037 problem, they cannot accept date after 2038/01/19.
[DateTimeOffset]$notAfter = [System.DateTimeOffset]::FromUnixTimeSeconds([Int32]::MaxValue)

# Try load CertificateRequest type dinamically to support multiple platforms...
$certReqType = [System.Linq.Enumerable].Assembly.GetType("System.Security.Cryptography.X509Certificates.CertificateRequest")
if ($null -eq $certReqType) {
	# This should be pwsh
	$certReqType = [X509Certificate].Assembly.GetType("System.Security.Cryptography.X509Certificates.CertificateRequest")
	if ($null -eq $certReqType) {
        throw [PlatformNotSupportedException]::new("pwsh, or PowerShell with .NET Framework 4.7.2 or later is required.");
    }
}

# Create CertificateRequest instance
$certReq = $certReqType::new($subject, $password, $hashAlgorithm, $padding)
# This cert is for code signing: 
$certReq.CertificateExtensions.Add([X509EnhancedKeyUsageExtension]::new([X509Extension]::new([Oid]::FromOidValue("2.5.29.37", [OidGroup]::All ), @(48, 10, 6, 8, 43, 6, 1, 5, 5, 7, 3, 3), $true), $true))
# This cert is end entity: 2.5.29.19, {48,0}, critical.
$certReq.CertificateExtensions.Add([X509BasicConstraintsExtension]::new([X509Extension]::new([Oid]::FromOidValue("2.5.29.19", [OidGroup]::All ), @(48, 0), $true), $true))

[X509Certificate2]$cert = $certReq.CreateSelfSigned($notBefore, $notAfter)
[File]::WriteAllBytes($FilePath, $cert.Export([X509ContentType]::Pfx, [String]::Empty))
