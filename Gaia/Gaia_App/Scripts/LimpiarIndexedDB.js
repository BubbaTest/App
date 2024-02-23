//var DBDeleteRequestNicaraoDAL = window.indexedDB.deleteDatabase("NicaraoDAL");
//var DBDeleteRequestDefensoriaDAL = window.indexedDB.deleteDatabase("DefensoriaDAL");

//DBDeleteRequestNicaraoDAL.onerror = function (event) {
//    console.log("Error deleting database.");
//};

//DBDeleteRequestNicaraoDAL.onsuccess = function (event) {
//    console.log("Database deleted successfully");

//    //console.log(request.result); // should be null
//};

//DBDeleteRequestDefensoriaDAL.onerror = function (event) {
//    console.log("Error deleting database.");
//};

//DBDeleteRequestDefensoriaDAL.onsuccess = function (event) {
//    console.log("Database deleted successfully");

//    //console.log(request.result); // should be null
//};

Dexie.exists('NicaraoDAL').then(function (exists) {
    if (exists) {
        var DBDeleteRequestNicaraoDAL = window.indexedDB.deleteDatabase("NicaraoDAL");

        DBDeleteRequestNicaraoDAL.onerror = function (event) {
            console.log("Error deleting database.");
        };

        DBDeleteRequestNicaraoDAL.onsuccess = function (event) {
            console.log("Database deleted successfully");

            //console.log(request.result); // should be null
        };
    }
});

//Dexie.exists('DefensoriaDAL').then(function (exists) {
//    if (exists) {
//        var DBDeleteRequestDefensoriaDAL = window.indexedDB.deleteDatabase("DefensoriaDAL");

//        DBDeleteRequestDefensoriaDAL.onerror = function (event) {
//            console.log("Error deleting database.");
//        };

//        DBDeleteRequestDefensoriaDAL.onsuccess = function (event) {
//            console.log("Database deleted successfully");

//            //console.log(request.result); // should be null
//        };

//    }
//});