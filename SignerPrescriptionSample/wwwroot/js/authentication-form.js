var authenticationForm = (function () {

	var pki = null;
	var formElements = {};
	var selectedCertThumbprint = null;

	// -------------------------------------------------------------------------------------------------
	// Function called once the page is loaded
	// -------------------------------------------------------------------------------------------------
	function init(fe) {

		formElements = fe;

		// Wireup of button clicks.
		formElements.signInButton.click(signIn);
		formElements.refreshButton.click(refresh);

		// Block the UI while we get things ready
		$.blockUI({ message: 'Initializing ...' });

		pki = new LacunaWebPKI("AhwAcHJlc2NyaWNhby5henVyZXdlYnNpdGVzLm5ldAAACABTdGFuZGFyZAgAANg2I9G53wgAAZJT4bGnyvb4ddQ3yuONJyhdxPAmsM9TcASebywtMWTXT3n/M3b7RxP4/JAmPP+M7xqFTN5e+nWsDGeyi38mpKsPQ+P4xWKNv7fkTPZNR41IMcNymtHSEZuSGuMhWnFr7ODiXDA7qAZnghzwQ5OdgZ3YIUiBWyOuhUVhodcz9z6mEA5yQiYOL2cVwDgwV6uiNm3KeEqTuZarbNP2BoHSau9/5skev9H25aVOG3SVpv3BIokNspQJrO0JGeOLxrXw1qmlw8wdIagCJ5vCXcEO+sBSwNMB+7ftX/Pfblp44pv6RYJ3GPHBmFtQguPRSHxoSB6YLRooLEDN/lHUNbRV9qo=");


		pki.init({
			ready: loadCertificates,
			defaultFail: onWebPkiError 
		});
	}

	function refresh() {

		$.blockUI({ message: 'Refreshing ...' });

		loadCertificates();
	}

	function loadCertificates() {


		pki.listCertificates({


			selectId: formElements.certificateSelect.attr('id'),


			selectOptionFormatter: function (cert) {
				var s = cert.subjectName + ' (issued by ' + cert.issuerName + ')';
				if (new Date() > cert.validityEnd) {
					s = '[EXPIRED] ' + s;
				}
				return s;
			}

		}).success(function () {

			$.unblockUI();

		});
	}

	function signIn() {
		$.blockUI({ message: 'Signing in ...' });

		selectedCertThumbprint = formElements.certificateSelect.val();

		pki.readCertificate(selectedCertThumbprint).success(function (certEncoded) {
			formElements.certificateField.val(certEncoded);
			pki.signData({
				thumbprint: selectedCertThumbprint,
				data: formElements.nonceField.val(),
				digestAlgorithm: formElements.digestAlgorithmField.val()
			}).success(function (signature) {
				formElements.signatureField.val(signature);
				formElements.form.submit();
			});
		});
	}


    function onWebPkiError(ex) {


        $.unblockUI();


        if (console) {
            console.log('Web PKI error originated at ' + ex.origin + ': (' + ex.code + ') ' + ex.error);
        }
    }

	return {
		init: init
	};
})();
