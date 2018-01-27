var jwtApiHelper = {
    baseURL: 'http://localhost:56440/api/',
    getSuppliersURL: function () {
        return this.baseURL + 'Suppliers';
    },
    getSupplierDetailsURL: function (supplierId) {
        return this.getSuppliersURL() + '/' + supplierId;
    },
    getRequestTokenURL: function () {
        return this.baseURL + 'Token/RequestToken';
    },
    token: ''
};

jQuery(function () {
    if (jQuery('#suppliers-table').length) {
        jQuery.ajax({
            url: jwtApiHelper.getSuppliersURL(),
            type: 'GET',
            contentType: 'application/json',
            success: function (suppliers) {
                for (var i in suppliers) {
                    var supplier = suppliers[i];
                    var supplier_row = jQuery('<tr></tr>');
                    supplier_row.append('<td>' + supplier.companyName + '</td>');
                    supplier_row.append('<td>' + supplier.city + '</td>');
                    supplier_row.append('<td>' + supplier.country + '</td>');
                    supplier_row.append('<td></td>');
                    supplier_row.append('<td></td>');
                    supplier_row.append('<td></td>');
                    supplier_row.append('<td></td>');
                    supplier_row.append('<td><button class="btn bnt-info" onclick="showDetails(jQuery(this));">Show details</button><input type="hidden" value="' + supplier.id + '" /></td>');
                    jQuery('#suppliers-table > tbody').append(supplier_row);
                }
            },
            error: function (err) {
                console.error('Greska kod spajanja na servis!');
            }
        });
    }
});

function showDetails(supplierBtn) {
    if (jwtApiHelper.token === '') {
        jQuery.ajax({
            url: jwtApiHelper.getRequestTokenURL(),
            type: 'POST',
            contentType: 'application/json; charset=UTF-8',
            headers: {
                'api-version': '1.0'
            },
            dataType: 'application/json',
            data: JSON.stringify({
                'FirstName': jQuery('#first-name').val(),
                'Phone': jQuery('#phone').val()
            }),
            success: tokenSuccess,
            error: function (err) {
                //console.error('Greska kod dohvata tokena!');
                jwtApiHelper.token = 'Bearer ' + err.responseText.substring(1, err.responseText.length - 1);
                var supplierId = supplierBtn.next().val();
                jQuery.ajax({
                    url: jwtApiHelper.getSupplierDetailsURL(supplierId),
                    type: 'GET',
                    contentType: 'application/json; charset=UTF-8',
                    headers: {
                        'Authorization': jwtApiHelper.token
                    },
                    success: function (supplier) {
                        var productCell = supplierBtn.parent().prev();
                        var faxCell = productCell.prev();
                        var phoneCell = faxCell.prev();
                        var contactCell = phoneCell.prev();

                        contactCell.html(supplier.contactName);
                        phoneCell.html(supplier.phone);
                        faxCell.html(supplier.fax);
                        productCell.html(supplier.product.length);
                    },
                    error: function (err) {
                        console.error('Greska kod spajanja na servis!');
                    }
                });
            }
        });
    } else {
        var supplierId = supplierBtn.next().val();
        jQuery.ajax({
            url: jwtApiHelper.getSupplierDetailsURL(supplierId),
            type: 'GET',
            contentType: 'application/json; charset=UTF-8',
            headers: {
                'Authorization': jwtApiHelper.token
            },
            success: function (supplier) {
                var productCell = supplierBtn.parent().prev();
                var faxCell = productCell.prev();
                var phoneCell = faxCell.prev();
                var contactCell = phoneCell.prev();

                contactCell.html(supplier.contactName);
                phoneCell.html(supplier.phone);
                faxCell.html(supplier.fax);
                productCell.html(supplier.product.length);
            },
            error: function (err) {
                console.error('Greska kod spajanja na servis!');
            }
        });
    }
}

function tokenSuccess() {

}