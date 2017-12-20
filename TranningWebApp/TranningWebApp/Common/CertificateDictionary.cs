using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using iTextSharp.text;
using TranningWebApp.Models;

namespace TmsWebApp.Common
{
    public class CertificateDictionary
    {
        Dictionary<string, List<PdfCoordinatesModel>> certificatesDictionary = new Dictionary<string, List<PdfCoordinatesModel>>();
        public CertificateDictionary()
        {
            certificatesDictionary.Add("CPCertificateStudent2016", new List<PdfCoordinatesModel>() {
                        new PdfCoordinatesModel { Alignment = Element.ALIGN_LEFT,X=290,Y = 380,CertifiacteData = CertificateEnum.NameOfStudent},
                        new PdfCoordinatesModel { Alignment = Element.ALIGN_LEFT,X=200,Y = 343,CertifiacteData = CertificateEnum.ProgrammYear },
                        new PdfCoordinatesModel { Alignment = Element.ALIGN_LEFT,X=282,Y = 246,CertifiacteData = CertificateEnum.NameOfStudent},
                        new PdfCoordinatesModel { Alignment = Element.ALIGN_LEFT,X=630,Y = 212,CertifiacteData = CertificateEnum.ProgrammYear}
                    });
            certificatesDictionary.Add("CPCertificateVolunteer2016", new List<PdfCoordinatesModel>() {
                        new PdfCoordinatesModel { Alignment = Element.ALIGN_LEFT,X=320,Y = 399,CertifiacteData = CertificateEnum.NameOfVolunteer},
                        new PdfCoordinatesModel { Alignment = Element.ALIGN_LEFT,X=312,Y = 342,CertifiacteData = CertificateEnum.TranningHour},
                        new PdfCoordinatesModel { Alignment = Element.ALIGN_LEFT,X=124,Y = 342,CertifiacteData = CertificateEnum.ProgrammYear},
                        new PdfCoordinatesModel { Alignment = Element.ALIGN_LEFT,X=270,Y = 269,CertifiacteData = CertificateEnum.NameOfVolunteer},
                        new PdfCoordinatesModel { Alignment = Element.ALIGN_LEFT,X=442,Y = 179,CertifiacteData = CertificateEnum.TranningHour},
                        new PdfCoordinatesModel { Alignment = Element.ALIGN_LEFT,X=536,Y = 179,CertifiacteData = CertificateEnum.ProgrammYear}
                    });

            certificatesDictionary.Add("PFCertificate_Student", new List<PdfCoordinatesModel>() {
                        new PdfCoordinatesModel { Alignment = Element.ALIGN_LEFT,X=313,Y = 313, CertifiacteData = CertificateEnum.NameOfStudent},
                        new PdfCoordinatesModel { Alignment = Element.ALIGN_LEFT,X=216,Y = 132, CertifiacteData = CertificateEnum.NameOfCoordinator},
                        new PdfCoordinatesModel { Alignment = Element.ALIGN_LEFT,X=695,Y = 132,CertifiacteData = CertificateEnum.NameOfVolunteer}
                    });
            certificatesDictionary.Add("PFCertificate_Volunteer", new List<PdfCoordinatesModel>() {
                        new PdfCoordinatesModel { Alignment = Element.ALIGN_LEFT,X=333,Y = 352, CertifiacteData = CertificateEnum.NameOfVolunteer},
                        new PdfCoordinatesModel { Alignment = Element.ALIGN_LEFT,X=540,Y = 222, CertifiacteData = CertificateEnum.TranningHour},
                        new PdfCoordinatesModel { Alignment = Element.ALIGN_LEFT,X=105,Y = 215, CertifiacteData = CertificateEnum.TranningDate}
                    });
            certificatesDictionary.Add("SafeerCertificate_Student", new List<PdfCoordinatesModel>() {
                new PdfCoordinatesModel { Alignment = Element.ALIGN_LEFT,X=313,Y = 313, CertifiacteData = CertificateEnum.NameOfStudent},
                new PdfCoordinatesModel { Alignment = Element.ALIGN_LEFT,X=216,Y = 132, CertifiacteData = CertificateEnum.NameOfCoordinator},
                new PdfCoordinatesModel { Alignment = Element.ALIGN_LEFT,X=695,Y = 132, CertifiacteData = CertificateEnum.NameOfVolunteer}
            });
            certificatesDictionary.Add("SafeerCertificate_Volunteer", new List<PdfCoordinatesModel>() {
                new PdfCoordinatesModel { Alignment = Element.ALIGN_LEFT,X=490,Y = 360, CertifiacteData = CertificateEnum.NameOfVolunteer},
                new PdfCoordinatesModel { Alignment = Element.ALIGN_LEFT,X=290,Y = 144, CertifiacteData = CertificateEnum.TranningHour},
                new PdfCoordinatesModel { Alignment = Element.ALIGN_LEFT,X=744,Y = 144, CertifiacteData = CertificateEnum.TranningDate}
            });
            certificatesDictionary.Add("SYCCertificates-Student", new List<PdfCoordinatesModel>() {
                new PdfCoordinatesModel { Alignment = Element.ALIGN_LEFT,X=324,Y = 362, CertifiacteData = CertificateEnum.NameOfStudent},
                //new PdfCoordinatesModel { Alignment = Element.ALIGN_LEFT,X=495,Y = 265 },
                new PdfCoordinatesModel { Alignment = Element.ALIGN_LEFT,X=191,Y = 151,CertifiacteData = CertificateEnum.NameOfVolunteer},
                new PdfCoordinatesModel { Alignment = Element.ALIGN_LEFT,X=671,Y = 151,CertifiacteData= CertificateEnum.NameOfCoordinator}
            });
            certificatesDictionary.Add("SYCCertificates-Volunteer", new List<PdfCoordinatesModel>() { 
                new PdfCoordinatesModel { Alignment = Element.ALIGN_RIGHT,X=405,Y = 350,CertifiacteData= CertificateEnum.NameOfVolunteer}
            });
            certificatesDictionary.Add("MrshidyCertificateStudent", new List<PdfCoordinatesModel>() {
                new PdfCoordinatesModel { Alignment = Element.ALIGN_LEFT,X=266,Y = 280, CertifiacteData = CertificateEnum.NameOfStudent},
                new PdfCoordinatesModel { Alignment = Element.ALIGN_LEFT,X=650,Y = 89 , CertifiacteData = CertificateEnum.NameOfCoordinator},
            });
            certificatesDictionary.Add("MrshidyCertificateVolunteer", new List<PdfCoordinatesModel>() {
                new PdfCoordinatesModel { Alignment = Element.ALIGN_LEFT,X=460,Y = 239, CertifiacteData = CertificateEnum.NameOfVolunteer},
                new PdfCoordinatesModel { Alignment = Element.ALIGN_LEFT,X=607,Y = 74 , CertifiacteData = CertificateEnum.TranningDate},
                new PdfCoordinatesModel { Alignment = Element.ALIGN_LEFT,X=232,Y = 74 , CertifiacteData = CertificateEnum.TranningHour}
            }); 
        }

        public List<PdfCoordinatesModel> GetPdfCoordinatesFromDictionary(string key)
        {
            return certificatesDictionary.Where(x => x.Key == key).Select(x => x.Value).FirstOrDefault();
        }

    }

}