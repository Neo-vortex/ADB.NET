using ADB.NET.Interfaces;
using ADB.NET.Utilities;

namespace ADB.NET.DataTypes.ABDpacket;

public static class ADBpacketFactory
{
 public static ADBpacket CreateConnectPacket(string hostname)
 {
  var header = ADBheaderFactory.CreateConnectHeader();
  var payload = System.Text.Encoding.UTF8.GetBytes(hostname);
  return new ADBpacket(header, new ADBdata(payload) );
 }

 public static ADBpacket CreateAuthBpacket(byte[] authData, IAuthenticationMethodParsable method)
 {
  var header = ADBheaderFactory.CreateAuthHeader(method);
  return new ADBpacket(header, new ADBdata(authData) );
 }
}