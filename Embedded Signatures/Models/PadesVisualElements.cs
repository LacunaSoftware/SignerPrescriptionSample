using Lacuna.RestPki.Api.PadesSignature;
using RestPki = Lacuna.RestPki.Client;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using Lacuna.RestPki.Client;

namespace PkiSuiteAspNetMvcSample.Classes {
    public class PadesVisualElements {



        // This function is called by the PAdES samples for REST PKI. It contains a example of signature visual
        // representation. This is only in a separate function in order to organize the various examples.
        public static RestPki.PadesVisualRepresentation GetVisualRepresentationForRestPki(RestPkiClient restPkiClient) {

            // Create a visual representation.
            var visualRepresentation = new RestPki.PadesVisualRepresentation() {
                // For a full list of the supported tags, see:
                // https://github.com/LacunaSoftware/RestPkiSamples/blob/master/PadesTags.md
                Text = new RestPki.PadesVisualText("Signed by {{name}} ({{national_id}})") {
                    FontSize = 13.0,
                    // Specify that the signing time should also be rendered.
                    IncludeSigningTime = true,
                    // Optionally, set the horizontal alignment of the text. If not set, the default is
                    // Left.
                    HorizontalAlign = PadesTextHorizontalAlign.Left,
                    // Optionally set the container within the signature rectangle on which to place the
                    // text. By default, the text can occupy the entire rectangle (how much of the
                    // rectangle the text will actually fill depends on the length and font size).
                    // Below, we specify that the text should respect a right margin of 1.5 cm.
                    Container = new RestPki.PadesVisualRectangle() {
                        Left = 0.2,
                        Top = 0.2,
                        Right = 0.2,
                        Bottom = 0.2
                    }
                },
                Image = new RestPki.PadesVisualImage(File.ReadAllBytes("D:\\Projetos\\SignerPrescriptionSample\\Embedded Signatures\\wwwroot\\PdfStamp.png"), "image/png") {
                    // Align image to the right horizontally.
                    HorizontalAlign = PadesHorizontalAlign.Right,
                    // Align image to center vertically.
                    VerticalAlign = PadesVerticalAlign.Center
                },
            };

            // Position of the visual represention. We get the footnote position preset and customize
            // it.
            var visualPositioning = RestPki.PadesVisualPositioning.GetFootnote(restPkiClient);
            visualPositioning.Container.Height = 4.94;
            visualPositioning.SignatureRectangleSize.Width = 8.0;
            visualPositioning.SignatureRectangleSize.Height = 4.94;
            visualRepresentation.Position = visualPositioning;

            return visualRepresentation;
        }

        // This function is called by the PAdES samples. It contains examples of PDF marks, visual
        // elements of arbitrary content placed in every page.
       
    }
}