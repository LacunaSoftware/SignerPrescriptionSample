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

		pki = new LacunaWebPKI("AhwAcHJlc2NyaWNhby5henVyZXdlYnNpdGVzLm5ldEMAaXA0OjEwLjAuMC4wLzgsaXA0OjEyNy4wLjAuMC84LGlwNDoxNzIuMTYuMC4wLzEyLGlwNDoxOTIuMTY4LjAuMC8xNgMAUHJvCAAAWBXUS43cCAABsB13MBOIOeU/cY3HAszYKqDDangfXM0XTQ7AT7/2fMb2BoEJR8QmztOQdgRz29nNptPjn1rrMWmTmGHKMuFQfPymLTEU3f8RjWmx89tSeBLu834elbE7fPCODXwRWHyl0HlaPTc/mJCUah5c2/yXBEKKu8EL0iE2ALpPgIUizMVp71cOgKDiNseHmNAGZ9U7g/92gTr1bI4uixhXrL4y2B7+bY9SU+Gfidf+yVcF695GcW/GPfmdAOcu6i2BtIFqOwyFlVpIHq3FaVOZ1Z0AXJOsf+pN/eO6+pHCzLOSua/dKGXdDKUAPlWtNujasINXJjTiC2hu51TTsp8NlpXxyw==");


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


        addAlert('danger', ex.userMessage);
    }

	return {
		init: init
	};
})();
