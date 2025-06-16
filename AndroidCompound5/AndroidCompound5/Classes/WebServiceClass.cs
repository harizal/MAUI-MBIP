using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using static Android.Net.Wifi.Hotspot2.Pps.Credential;

namespace AndroidCompound5.Classes
{
	public class WebServiceClass
	{
		//private string _urlService;
		private ServiceCompoundSoap _serviceCompound;
		private UserCredentials _userCredentials;

		public WebServiceClass()
		{
			_serviceCompound = null;

			var binding = new BasicHttpBinding();
			binding.Name = "MyServicesSoap";
			binding.CloseTimeout = TimeSpan.FromMinutes(1);
			binding.OpenTimeout = TimeSpan.FromMinutes(1);
			binding.ReceiveTimeout = TimeSpan.FromMinutes(10);
			binding.SendTimeout = TimeSpan.FromMinutes(1);
			//binding.AllowCookies = false;
			binding.BypassProxyOnLocal = false;
			//binding.HostNameComparisonMode = HostNameComparisonMode.StrongWildcard;
			binding.MaxBufferSize = 2147483647;
			binding.MaxReceivedMessageSize = 2147483647;
			binding.MaxBufferPoolSize = 2147483647;
			binding.MessageEncoding = WSMessageEncoding.Text;
			binding.TextEncoding = System.Text.Encoding.UTF8;
			binding.TransferMode = TransferMode.Buffered;
			binding.UseDefaultWebProxy = true;

			binding.Security.Mode = BasicHttpSecurityMode.None;
			binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.None;

			//var endpoint = new EndpointAddress(GlobalClass.ServiceUrl);

			//_serviceCompound = new ServiceCompoundSoapClient(binding, endpoint);

			//_userCredentials = new UserCredentials();
			//_userCredentials.userid = Encryption.Encrypt(GlobalClass.ServiceUser);
			//_userCredentials.password = Encryption.Encrypt(GlobalClass.ServicePassword);
		}



		//public string TestService()
		//{
		//    string message = "";
		//    try
		//    {
		//        var result = _serviceCompound.TestServiceCall(_userCredentials);
		//        message = result;
		//    }
		//    catch (Exception ex)
		//    {
		//        message = ex.Message;
		//    }

		//    return message;
		//}

		//public InsertCompoundResult SendCompoundToService(CompoundInfo compoundInfo)
		//{
		//    var compoundResult = new InsertCompoundResult();

		//    try
		//    {
		//        if (!GeneralClass.IsNumeric(compoundInfo.amnkmp))
		//            compoundInfo.amnkmp = "0";
		//        if (!GeneralClass.IsNumeric(compoundInfo.amnkmp2))
		//            compoundInfo.amnkmp2 = "0";
		//        if (!GeneralClass.IsNumeric(compoundInfo.amnkmp3))
		//            compoundInfo.amnkmp3 = "0";
		//        if (!GeneralClass.IsNumeric(compoundInfo.PrintCnt))
		//            compoundInfo.PrintCnt = "0";

		//        var result = _serviceCompound.InsertCompoundDetailsMBSP(compoundInfo, _userCredentials);

		//        compoundResult.Response = true;
		//        compoundResult.MessageResponse = result;

		//    }
		//    catch (Exception ex)
		//    {
		//        compoundResult.Response = false;
		//        compoundResult.MessageResponse = ex.Message;
		//        LogBll.WriteLogFile(LogType.CompoundService, "Error SendCompoundToService() : " + ex.Message);
		//    }

		//    return compoundResult;
		//}



		private CompoundInfo SetCompoundInfo(StructClass.compound_t compoundData)
		{
			var compoundInfo = new CompoundInfo();

			//general
			compoundInfo.CompoundNumber = compoundData.CompNum;
			compoundInfo.CompoundType = compoundData.CompType;
			compoundInfo.Actcode = compoundData.ActCode;
			compoundInfo.Kodakta = compoundData.OfdCode;
			compoundInfo.Mukim = compoundData.Mukim;
			compoundInfo.Zone = compoundData.Zone;
			compoundInfo.street = compoundData.StreetCode;
			compoundInfo.streetDescr = compoundData.StreetDesc;
			compoundInfo.enforcer = compoundData.EnforcerId;
			compoundInfo.witness = compoundData.WitnessId1;
			compoundInfo.PubWitness = compoundData.PubWitness;
			compoundInfo.PrintCnt = compoundData.PrintCnt;
			compoundInfo.Tempatjadi = compoundData.Tempatjadi;
			compoundInfo.IssueDate = compoundData.IssueDate;
			compoundInfo.IssueTime = compoundData.IssueTime;
			compoundInfo.Kadar = compoundData.Kadar;
			compoundInfo.NamaPenerima = compoundData.NamaPenerima;
			compoundInfo.IcPenerima = compoundData.IcPenerima;
			compoundInfo.compDate = compoundData.CompDate;
			compoundInfo.compTime = compoundData.CompTime;
			compoundInfo.kodSalah = compoundData.OfdCode;

			//compoundInfo.Longitude = GlobalClass.Longitude;
			//compoundInfo.Latitude = GlobalClass.Latitude;

			//switch (compoundData.CompType)
			//{
			//    case GlobalClass.COMP_TYPE1:
			//        compoundInfo.CarType = compoundData.Type1.CarType;
			//        compoundInfo.CarTypeDesc = compoundData.Type1.CarTypeDesc;
			//        compoundInfo.CarColor = compoundData.Type1.CarColor;
			//        compoundInfo.CarColorDesc = compoundData.Type1.CarColorDesc;
			//        compoundInfo.RoadTax = compoundData.Type1.RoadTax;
			//        compoundInfo.ParkingLot = compoundData.Type1.ParkingLot;
			//        compoundInfo.amnkmp = compoundData.Type1.CompAmt;
			//        compoundInfo.amnkmp2 = compoundData.Type1.CompAmt2;
			//        compoundInfo.amnkmp3 = compoundData.Type1.CompAmt3;
			//        compoundInfo.compDescr = compoundData.Type1.CompDesc;
			//        break;
			//    case GlobalClass.COMP_TYPE2:
			//        compoundInfo.CarType = compoundData.Type2.CarType;
			//        compoundInfo.CarTypeDesc = compoundData.Type2.CarTypeDesc;
			//        compoundInfo.CarColor = compoundData.Type2.CarColor;
			//        compoundInfo.CarColorDesc = compoundData.Type2.CarColorDesc;
			//        compoundInfo.RoadTax = compoundData.Type2.RoadTax;
			//        compoundInfo.amnkmp = compoundData.Type2.CompAmt;
			//        compoundInfo.amnkmp2 = compoundData.Type2.CompAmt2;
			//        compoundInfo.amnkmp3 = compoundData.Type2.CompAmt3;
			//        compoundInfo.DeliveryCode = compoundData.Type2.DeliveryCode;
			//        compoundInfo.compDescr = compoundData.Type2.CompDesc;
			//        compoundInfo.jenken = compoundData.Type2.Category;
			//        compoundInfo.noDaftar = compoundData.Type2.CarNum;
			//        break;
			//    case GlobalClass.COMP_TYPE3:
			//        compoundInfo.noRujukan = compoundData.Type3.Rujukan;
			//        compoundInfo.Company = compoundData.Type3.Company;
			//        compoundInfo.CompanyName = compoundData.Type3.CompanyName;
			//        compoundInfo.OffenderIc = compoundData.Type3.OffenderIc;
			//        compoundInfo.OffenderName = compoundData.Type3.OffenderName;
			//        compoundInfo.Alamat1 = compoundData.Type3.Alamat1;
			//        compoundInfo.alamat2 = compoundData.Type3.Alamat2;
			//        compoundInfo.alamat3 = compoundData.Type3.Alamat3;
			//        compoundInfo.amnkmp = compoundData.Type3.CompAmt;
			//        compoundInfo.amnkmp2 = compoundData.Type3.CompAmt2;
			//        compoundInfo.amnkmp3 = compoundData.Type3.CompAmt3;
			//        compoundInfo.DeliveryCode = compoundData.Type3.DeliveryCode;
			//        compoundInfo.compDescr = compoundData.Type3.CompDesc;
			//        break;
			//}

			return compoundInfo;

		}


		//private CompoundInfo SetCompoundInfo(CompoundClass compoundData)
		//{
		//    var compoundInfo = new CompoundInfo();

		//    //general
		//    compoundInfo.CompoundNumber = compoundData.GetCompoundNumber();
		//    compoundInfo.CompoundType = compoundData.GetCompType();
		//    compoundInfo.Actcode = compoundData.GetActCode();
		//    compoundInfo.Kodakta = compoundData.GetOfdCode(); 
		//    compoundInfo.Mukim = compoundData.GetMukim();
		//    compoundInfo.Zone = compoundData.GetZone();
		//    compoundInfo.street = compoundData.GetStreetCode();
		//    compoundInfo.streetDescr = compoundData.GetStreetDesc();
		//    compoundInfo.enforcer = compoundData.GetEnforcerId();
		//    compoundInfo.witness = compoundData.GetWitnessId1();
		//    compoundInfo.PubWitness = compoundData.GetPubWitness();
		//    compoundInfo.PrintCnt = compoundData.GetPrintCnt();
		//    compoundInfo.Tempatjadi = compoundData.GetTempatjadi();
		//    compoundInfo.IssueDate = compoundData.GetIssueDate();
		//    compoundInfo.IssueTime = compoundData.GetIssueTime();
		//    compoundInfo.Kadar = compoundData.GetKadar();
		//    compoundInfo.NamaPenerima = compoundData.GetNamaPenerima();
		//    compoundInfo.IcPenerima = compoundData.GetIcPenerima();
		//    compoundInfo.compDate = compoundData.GetCompDate();
		//    compoundInfo.compTime = compoundData.GetCompTime();
		//    compoundInfo.kodSalah = compoundData.GetOfdCode();
		//    switch (compoundData.GetCompType())
		//    {
		//        case GlobalClass.COMP_TYPE1:
		//            compoundInfo.CarType = compoundData.GetCarType();
		//            compoundInfo.CarTypeDesc = compoundData.GetCarTypeDesc();
		//            compoundInfo.CarColor = compoundData.GetCarColor();
		//            compoundInfo.CarColorDesc = compoundData.GetCarColorDesc();
		//            compoundInfo.RoadTax = compoundData.GetRoadTax();
		//            compoundInfo.amnkmp = compoundData.GetCompAmt();
		//            compoundInfo.amnkmp2 = compoundData.GetCompAmt2();
		//            compoundInfo.amnkmp3 = compoundData.GetCompAmt3();
		//            compoundInfo.compDescr = compoundData.GetCompDesc();
		//            break;
		//        case GlobalClass.COMP_TYPE2:
		//            compoundInfo.CarType = compoundData.GetCarType();
		//            compoundInfo.CarTypeDesc = compoundData.GetCarTypeDesc();
		//            compoundInfo.CarColor = compoundData.GetCarColor();
		//            compoundInfo.CarColorDesc = compoundData.GetCarColorDesc();
		//            compoundInfo.RoadTax = compoundData.GetRoadTax();
		//            compoundInfo.amnkmp = compoundData.GetCompAmt();
		//            compoundInfo.amnkmp2 = compoundData.GetCompAmt2();
		//            compoundInfo.amnkmp3 = compoundData.GetCompAmt3();
		//            compoundInfo.DeliveryCode = compoundData.GetDeliveryCode();
		//            compoundInfo.compDescr = compoundData.GetCompDesc();
		//            compoundInfo.jenken = compoundData.GetCarCategory();
		//            break;
		//        case GlobalClass.COMP_TYPE3:
		//            compoundInfo.noRujukan = compoundData.GetRujukan();
		//            compoundInfo.Company = compoundData.GetCompany();
		//            compoundInfo.CompanyName = compoundData.GetCompanyName();
		//            compoundInfo.OffenderIc = compoundData.GetOffenderIc();
		//            compoundInfo.OffenderName = compoundData.GetOffenderName();
		//            compoundInfo.Alamat1 = compoundData.GetAlamat1();
		//            compoundInfo.alamat2 = compoundData.GetAlamat2();
		//            compoundInfo.alamat3 = compoundData.GetAlamat3();
		//            compoundInfo.amnkmp = compoundData.GetCompAmt();
		//            compoundInfo.amnkmp2 = compoundData.GetCompAmt2();
		//            compoundInfo.amnkmp3 = compoundData.GetCompAmt3();
		//            compoundInfo.DeliveryCode = compoundData.GetDeliveryCode();
		//            compoundInfo.compDescr = compoundData.GetCompDesc();
		//            break;
		//    }

		//    return compoundInfo;

		//}


		//private void SendFailedCompound(string compoundNumber)
		//{
		//    try
		//    {
		//        var listCompound = GetFailedCompound(compoundNumber);
		//        foreach (var compoundT in listCompound)
		//        {
		//            var compoundInfo = SetCompoundInfo(compoundT);
		//            var compoundResult = SendCompoundToService(compoundInfo);
		//            if (compoundResult.MessageResponse.ToLower() != "success")
		//                return;
		//            //update compound.fil
		//            UpdateCompoundAfterSend(compoundInfo.CompoundNumber, true);
		//        }
		//    }
		//    catch (Exception ex)
		//    {
		//        return;
		//    }

		//}

		//private List<StructClass.compound_t> GetFailedCompound(string compoundNumber)
		//{

		//    var outputResult = new List<StructClass.compound_t>();

		//    StructClass.compound_t CompoundRead;
		//    StringBuilder result = new StringBuilder();

		//    StreamReader objCompound = FileClass.ReadFile(GlobalClass.TRANSPATH + GlobalClass.COMPOUNDFIL);

		//    string sLine;
		//    //search compound
		//    while ((sLine = objCompound.ReadLine()) != null)
		//    {
		//        CompoundRead = new StructClass.compound_t();
		//        GeneralClass.ReadDataCompound(ref CompoundRead, sLine);
		//        //if found , replace the line with new compound that updated
		//        if (CompoundRead.CompNum != compoundNumber && CompoundRead.Deleted == GlobalClass.REC_FAILEDSENDSERVICE)
		//        {
		//           outputResult.Add(CompoundRead);
		//        }
		//    }
		//    objCompound.Close();
		//    objCompound.Dispose();

		//    return outputResult;
		//}

		//private StructClass.compound_t GetCompoundByCompoundNumber(string compoundNumber)
		//{
		//    var compoundResult = new StructClass.compound_t();

		//    StreamReader objCompound = FileClass.ReadFile(GlobalClass.TRANSPATH + GlobalClass.COMPOUNDFIL);

		//    string sLine;
		//    //search compound
		//    while ((sLine = objCompound.ReadLine()) != null)
		//    {
		//        var compoundRead = new StructClass.compound_t();
		//        GeneralClass.ReadDataCompound(ref compoundRead, sLine);

		//        if (compoundRead.CompNum == compoundNumber)
		//        {
		//            compoundResult = compoundRead;
		//            break;
		//        }
		//    }
		//    objCompound.Close();
		//    objCompound.Dispose();

		//    return compoundResult;
		//}

		//private void UpdateCompoundAfterSend(string compoundNumber, bool isSuccess)
		//{
		//    StreamReader objCompound = FileClass.ReadFile(GlobalClass.TRANSPATH + GlobalClass.COMPOUNDFIL);
		//    StringBuilder result = new StringBuilder();

		//    string sLine;
		//    //search compound
		//    while ((sLine = objCompound.ReadLine()) != null)
		//    {
		//        var compoundRead = new StructClass.compound_t();
		//        GeneralClass.ReadDataCompound(ref compoundRead, sLine);

		//        if (compoundRead.CompNum == compoundNumber)
		//        {
		//            if (isSuccess)
		//                compoundRead.Deleted = GlobalClass.REC_ACTIVE;
		//            else
		//                compoundRead.Deleted = GlobalClass.REC_FAILEDSENDSERVICE;

		//            result.Append(GeneralClass.ReadDataCompound(compoundRead) + GlobalClass.NewLine);
		//        }
		//        else
		//            result.Append(sLine + GlobalClass.NewLine);
		//    }
		//    objCompound.Close();
		//    objCompound.Dispose();

		//    //StreamWriter objWrite = GlobalClass.FileSystemAndroid.WriteInfoFile(GlobalClass.TRANSPATH + GlobalClass.COMPOUNDFIL);
		//    StreamWriter objWrite = FileClass.WriteFile(GlobalClass.TRANS_SPATH + GlobalClass.COMPOUNDFIL);

		//    objWrite.Write(result);
		//    objWrite.Close();
		//    objWrite.Dispose();
		//}

		//public string SendGps(GpsDto gpsDto)
		//{
		//    string result = "";
		//    try
		//    {
		//        var gpsInfo = SetGpsInfo(gpsDto);

		//        result = _serviceCompound.InsertEnforceractivity(gpsInfo, _userCredentials);

		//        return result;
		//    }
		//    catch (Exception ex)
		//    {
		//        LogBll.WriteLogFile(LogType.GpsService, "Error SendGpsInfo() : " + ex.Message);
		//        return ex.Message;
		//    }
		//}

		//private GPSInfo SetGpsInfo(GpsDto gpsDto)
		//{
		//    var gpsInfo = new GPSInfo();
		//    gpsInfo.ActivityDate = gpsDto.ActivityDate;
		//    gpsInfo.ActivityTime = gpsDto.ActivityTime;
		//    gpsInfo.Batterylife = gpsDto.BatteryLife;
		//    gpsInfo.EnforcerID = gpsDto.Kodpguatkuasa;
		//    gpsInfo.NoDolphin = gpsDto.DhId;
		//    gpsInfo.Latitude = gpsDto.GpsX;
		//    gpsInfo.Longitude = gpsDto.GpsY;

		//    return gpsInfo;
		//}



	}
}