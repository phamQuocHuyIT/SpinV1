$(function () {
    var l = abp.localization.getResource('AbpSolution1');
    console.log("JS loaded");
    // Hide Customer_Id field
    var customer_Id = $('#Customer_Id'); 
    customer_Id.closest('.mb-3').hide();
    // Synchronize GenderStatus with Customer
    var genderStatus = $('#GenderStatus');               
    var customer_Gender = $('#Customer_Gender'); 


    customer_Gender.closest('.mb-3').hide();
    

    customer_Gender.val(genderStatus.val());

    genderStatus.on('change', function () {
        var selectedValue = genderStatus.val();
        console.log("Selected Gender:", selectedValue);
        customer_Gender.val(selectedValue);
    });

    var rankesStatus = $('#RankesStatus');
    var customer_Ranked = $('#Customer_Ranked');


    customer_Ranked.closest('.mb-3').hide();


    customer_Ranked.val(rankesStatus.val());

    rankesStatus.on('change', function () {
        var selectedValue = rankesStatus.val();
        console.log("Selected Gender:", selectedValue);
        customer_Ranked.val(selectedValue);
    });
});
