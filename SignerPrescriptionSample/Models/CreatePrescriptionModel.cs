using Lacuna.Pki;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace SignerPrescriptionSample.Models {
    public class CreatePrescriptionModel {
        [Required(ErrorMessage = "É necessário informar um CPF")]
        public string Identifier { get; set; }
        [Required(ErrorMessage = "É necessário informar um nome")]
        public string Name { get; set; }
        [Required(ErrorMessage = "É necessário informar um e-mail válido")]
        public string Email { get; set; }
        [Required(ErrorMessage = "É necessário informar o nome do paciente")]
        public string PatientName { get; set; }
        [Required(ErrorMessage = "É necessário informar o CPF do paciente.")]
        [StringLength(14, ErrorMessage = "O CPF deve ter 11 dígitos.")]
        [CPFValidation(ErrorMessage = "CPF inválido.")]
        public string PatientIdentifier { get; set; }
        [Required(ErrorMessage = "É necessário informar o nome do medicamento")]
        public string MedicationName { get; set; }
        [Required(ErrorMessage = "É necessário informar a posologia do medicamento")]
        public string MedicationDosage { get; set; }
        [Required(ErrorMessage = "É necessário informar a quantidade do medicamento")]
        public string MedicationQuantity { get; set; }
        public bool AllowElectronicSignature { get; set; }
        [Required(ErrorMessage = "É necessário informar a unidade federativa")]
        public PKCertificate? UserCert { get; set; }
        public string? CpfManual { get; set; }
        public string UF { get; set; }
        [Required(ErrorMessage = "É necessário informar um CRM válido")]
        public string CRM { get; set; }


        public static List<SelectListItem> GetUFs() {
            return new List<SelectListItem>
            {
            new SelectListItem { Text = "Acre", Value = "AC" },
            new SelectListItem { Text = "Alagoas", Value = "AL" },
            new SelectListItem { Text = "Amapá", Value = "AP" },
            new SelectListItem { Text = "Amazonas", Value = "AM" },
            new SelectListItem { Text = "Bahia", Value = "BA" },
            new SelectListItem { Text = "Ceará", Value = "CE" },
            new SelectListItem { Text = "Distrito Federal", Value = "DF" },
            new SelectListItem { Text = "Espírito Santo", Value = "ES" },
            new SelectListItem { Text = "Goiás", Value = "GO" },
            new SelectListItem { Text = "Maranhão", Value = "MA" },
            new SelectListItem { Text = "Mato Grosso", Value = "MT" },
            new SelectListItem { Text = "Mato Grosso do Sul", Value = "MS" },
            new SelectListItem { Text = "Minas Gerais", Value = "MG" },
            new SelectListItem { Text = "Pará", Value = "PA" },
            new SelectListItem { Text = "Paraíba", Value = "PB" },
            new SelectListItem { Text = "Paraná", Value = "PR" },
            new SelectListItem { Text = "Pernambuco", Value = "PE" },
            new SelectListItem { Text = "Piauí", Value = "PI" },
            new SelectListItem { Text = "Rio de Janeiro", Value = "RJ" },
            new SelectListItem { Text = "Rio Grande do Norte", Value = "RN" },
            new SelectListItem { Text = "Rio Grande do Sul", Value = "RS" },
            new SelectListItem { Text = "Rondônia", Value = "RO" },
            new SelectListItem { Text = "Roraima", Value = "RR" },
            new SelectListItem { Text = "Santa Catarina", Value = "SC" },
            new SelectListItem { Text = "São Paulo", Value = "SP" },
            new SelectListItem { Text = "Sergipe", Value = "SE" },
            new SelectListItem { Text = "Tocantins", Value = "TO" }
        };
        }

        public class CPFValidationAttribute : ValidationAttribute {
            public override bool IsValid(object value) {
                if (value == null) return false;

                var cpf = value.ToString();

                // Remove todos os caracteres que não são dígitos
                cpf = Regex.Replace(cpf, "[^0-9]", "");

                if (cpf.Length != 11) return false;

                // Verifica se todos os dígitos são iguais
                if (new string(cpf[0], 11) == cpf) return false;

                // Realiza a validação de CPF com base nos dígitos verificadores
                int[] multiplicador1 = new int[9] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
                int[] multiplicador2 = new int[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };
                string tempCpf;
                string digito;
                int soma;
                int resto;

                tempCpf = cpf.Substring(0, 9);
                soma = 0;

                for (int i = 0; i < 9; i++)
                    soma += int.Parse(tempCpf[i].ToString()) * multiplicador1[i];

                resto = soma % 11;
                if (resto < 2)
                    resto = 0;
                else
                    resto = 11 - resto;

                digito = resto.ToString();
                tempCpf = tempCpf + digito;
                soma = 0;

                for (int i = 0; i < 10; i++)
                    soma += int.Parse(tempCpf[i].ToString()) * multiplicador2[i];

                resto = soma % 11;
                if (resto < 2)
                    resto = 0;
                else
                    resto = 11 - resto;

                digito = digito + resto.ToString();

                return cpf.EndsWith(digito);
            }
        }
    }
}
