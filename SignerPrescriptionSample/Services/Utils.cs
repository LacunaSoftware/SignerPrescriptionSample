﻿using Lacuna.Pki;
using Lacuna.Pki.Stores;

namespace SignerPrescriptionSample.Services
{
	public class Utils
	{

		public static INonceStore GetNonceStore(IWebHostEnvironment _webHostEnvironment)
		{

			return new FileSystemNonceStore(Path.Combine(_webHostEnvironment.ContentRootPath, "~/App_Data"));

		}
		public static ITrustArbitrator GetTrustArbitrator()
		{

			var trustArbitrator = new LinkedTrustArbitrator(TrustArbitrators.PkiBrazil, TrustArbitrators.Windows);

            var lacunaRootV3 = Lacuna.Pki.PKCertificate.Decode(Convert.FromBase64String("MIIFnjCCA4agAwIBAgIBATANBgkqhkiG9w0BAQ0FADBfMQswCQYDVQQGEwJCUjETMBEGA1UEChMKSUNQLUJyYXNpbDEdMBsGA1UECxMUTGFjdW5hIFNvZnR3YXJlIC0gTFMxHDAaBgNVBAMTE0xhY3VuYSBSb290IFRlc3QgdjMwIBcNMjQxMjE2MjA0MjEwWhgPMjA3NDEyMTYwMzAwMDBaMF8xCzAJBgNVBAYTAkJSMRMwEQYDVQQKEwpJQ1AtQnJhc2lsMR0wGwYDVQQLExRMYWN1bmEgU29mdHdhcmUgLSBMUzEcMBoGA1UEAxMTTGFjdW5hIFJvb3QgVGVzdCB2MzCCAiIwDQYJKoZIhvcNAQEBBQADggIPADCCAgoCggIBAN2j+WJCzBe0/PCCXClKQy89kfdFmSyH3wYDsMyUSIBFroeRhv5DEUWMIJcq0fx8bwXIfHv7FQEjPO91GyS3ke2f9uvT0qnAvB2hlwCqF5/eU+LuJYtgxc7vW3QCiUNsYkiHysnIvOB46YwmwOMcpi01eQjrkgsYBzBa4TYZymhD28oGT9VEXepVpzcxF/H1zAnBHrpQRTPz5AjsJ7x3IKOLWFScTCtTnpp4HZslBvnUIwiTQNu4HgsUOekRfDbVwdftcwFHFmW5Z3w6zJvJ/b1x7iv7+g2lskWBNzEV769KoOT6+uwr1zk6Zwv/Oeze22GXuWrKKqLVanvgSCMPSFFlwGYOyjfpXC6Ccwe7Ptnb3cfhvX4V3BtXxkcBvfT34jhT6eoqT6RPqCtA18YTF9qLXHxnQ/AmqJdgsu0+JBWGIQWj95/qv7Y2Q47/7Q6866Bp6SbWDWLd161l4IYe/R9DE8gsujGey9gydiNrtzxNW3BrtssmjnYs9no3X/tRvXLCA7A0jebjqQBg91CamveU0Ou5Cz7uzR5OUsGNuyIFLZSXc2v+WGhjGAEyNSO9Mqwl+Lm8huyzmiwtoyIfMWsKTHbp4iQwqAWkQYcmMTXip/+lJOVi3yQSNOs5o7GyWsPtKZRglUs2cVnhe6dk0Ke/y8j+1MevmBnvZ1SQQoOpAgMBAAGjYzBhMA8GA1UdEwEB/wQFMAMBAf8wHQYDVR0OBBYEFHKswdeMDNW5PgtBsXnjJLEdoEotMB8GA1UdIwQYMBaAFHKswdeMDNW5PgtBsXnjJLEdoEotMA4GA1UdDwEB/wQEAwIBBjANBgkqhkiG9w0BAQ0FAAOCAgEAhjObhPicGpi1iXbixX4yNIQj39XWnbY4f1j/SsSkQFBqnFF6vfleyaf4YridawGJcrXv1iX/KOeFLs02+f/rTH+wDmWs8a0mImyqOVW/Vn4gs/oSHu2ynTr/l9exvV0CLAqS7GPSj1CZ+j5gaFsq/NQxGImAHr/zr5E/XIgkwyF9PwGBAnO7QRj6n5au3swm0S+KqNSdaRtQkq61yZKdHxGS6K2bQAx88gWqWmiQ+P6XqKBn87+zTe5PsAqwvyM1sd9/oa0IF58o6g48dnxnpdMLs9KbB3/Rzh3n33JoQkjg2ZXyXk8fhefdAr18YKrmL9aX9q2GK1bnSGUPJkzbMbLIKpkAyboXG/zdbPDg3B7keZGdCgXrrBl1zoP3klieqclQJ0gWMVx5IbJfo/MmtgxbVNtd/CezXAESaCaApBQVC0U9GUVvekN6OrBYhkwA+HVbmF1fRznL05gV091Uc2iYOV+hiNrAJHvXuPxPVgqd2Mrx+9xc8VGOT6jGWkGAOEHbW1uVeZNcWTsHg9eQbRwiSUouR7zrMy+be/xMDloYJLE/94VuoGS8/Z2FS95HZ183J82Ihf2F2zsK485cmY4Unipq98yiHWtRWbaihQe1Dzfp1Av1U9gAMbOL0ounKC0dxstpNqfzC+z9nBwQpAviqUzghQIom9Q6aDk4gZ8="));
            trustArbitrator.Add(new TrustedRoots(lacunaRootV3));
            return trustArbitrator;


		}
	}
}

