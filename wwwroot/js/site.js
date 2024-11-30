// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.


//for +-button




//function minusval(x, y,z,t) {
//    console.log(x+"_____");
//    console.log(y+"_____");
//    console.log(z+"_____");
//    console.log(t+"----");
//    let num = document.getElementById(y).value;
//    let price = document.getElementById(z).innerHTML;
//    console.log(num+"movnum");
//    console.log(price + "price");

//    let curnum=num-1;
//    console.log("curnum");
//    console.log(curnum + "curnum");

//    if (curnum == 0)
//    {
//    console.log("lost");
//        console.log(x + "_____");

//        document.getElementById(x).remove();
//    }

//    else
//    {
//    let total = price * curnum;
//    console.log(total);

//    document.getElementById(y).value = curnum;
//    document.getElementById(t).innerHTML = `total: ${total}`;
//    }


//}

//function plusval(x, y, z, t,movieId) {
//    console.log(x + "_____");
//    console.log(y + "_____");
//    console.log(z + "_____");
//    console.log(t + "----");

//    let num = document.getElementById(y).value;
//    let price = document.getElementById(z).innerHTML;
//    console.log(num + "movnum");
//    console.log(price + "price");

//    let curnum = Number(num )+ 1;
//    console.log("curnum");
//    console.log(curnum + "curnum");
//    let total = price * curnum;

//    console.log("lost");

//    document.getElementById(y).value = curnum;
//    document.getElementById(t).innerHTML = `total: ${total}`;


//}

//function removediv(x) {
//    console.log(x);
//    document.getElementById(x).remove();


//}
function showHideOrderlist(divid) {
    //  console.log("div____" + divid);

    //var button = document.querySelector('.arrowBtn');
    //var arrow = button.querySelector('.arrowIcon');
    var div = document.getElementById(divid);
    //console.log(button);
    //console.log(div);
    if (div.style.display === 'none') {
        //console.log("any......");
        //arrow.innerHTML = '&#9650;'; //change to up arrow key
        div.style.display = 'block';
    }
    else {
        //console.log("thing......");

        div.style.display = 'none';
        //arrow.innerHTML = '&#9660;';//change to down arrow key

    }

}
function minusval1(movieId, x, y, z) {

    $.ajax({

        type: 'post',

        url: '/Customer/DecreaseItemInCart',

        dataType: 'json',

        data: { id: movieId },

        success: function (data) {
            if (data.numMov != 0) {

                $(`#${y}`).html(data.pris * data.numMov);
                //$("#totalcost").html(data.total);
                //$("#cartCount").html(data.totMovNum);
                console.log("success");

                document.getElementById(x).value = data.numMov;
                document.getElementById("totalcost").innerHTML = data.total;
                document.getElementById("cartCount").innerHTML = data.totMovNum;
            }
            else {
                if (data.totMovNum == 0) {
                    document.getElementById("totaldiv").style.display = "none";
                    document.getElementById("cartCount").style.display = "none";
                    document.getElementById("cartCount").style.display = "none";
                    $(`#${z}`).remove();
                }
                else {
                    document.getElementById("totalcost").innerHTML = data.total;
                    console.log(z);
                    $(`#${z}`).remove();
                    document.getElementById("cartCount").innerHTML = data.totMovNum;
                }

            }
        },

        error: function () {
            console.log('error');
        }

    });
}

function plusval1(movieId, x, y) {


    let price = Number(document.getElementById(y).innerHTML);
    console.log("abd " + price);

    $.ajax({

        type: 'post',

        url: '/Customer/IncreaseItemInCart',

        dataType: 'json',

        data: { id: movieId },

        success: function (data) {

            $(`#${y}`).html(data.pris * data.numMov);
            $("#totalcost").html(data.total);
            $("#cartCount").html(data.totMovNum);

            document.getElementById(x).value = data.numMov;
            //document.getElementById("totalcost").innerHTML = data.total;
            //document.getElementById("cartCount").innerHTML = data.totMovNum;
        },

        error: function (err) {
            console.log('error');
        }

    });
}





function addtoCartO(x) {

    $.ajax({

        type: 'post',

        url: '/Customer/AddToCart',

        dataType: 'json',

        data: { id: x },

        success: function (count) {

            $('#cartCount').html(count); // The id ‘cartCount’ refers to an HTML-element

        }

    });
}

function removediv(movid, x) {
    $.ajax({
        type: 'post',
        url: '/Customer/RemoveFromCart',
        dataType: 'json',
        data: { id: movid },
        success: function (data) {

            $("#totalcost").html(data.total);
            $("#cartCount").html(data.totMovNum);
            document.getElementById(x).style.display = "none";


        },
        error: function (err) {
            console.log(error);
        }
    });

}

function DisplayLoginPartial() {
    document.getElementById("displayLogReg").style.display = "block";

    $.ajax({

        url: '/Customer/DisplayLoginPartiaView',
        success: function (result) {
            $("#displayLogReg").html(result);


        },
        error: function (err) {
            console.log(err);

        }
    });

}

function DisplayRegisterPartial() {
    document.getElementById("displayLogReg").style.display = "block";

    $.ajax({

        url: '/Customer/DisplayRegistraionPartiaView',
        success: function (result) {
            $("#displayLogReg").html(result);


        },
        error: function (err) {
            console.log(err);

        }
    });

}



function CheckOutFromCart() {
    $.ajax({
        url: '/Customer/CheckoutfromCart',
        //data: { id: cusid },
        dataType: 'json',

        success: function (data) {
            if (data.success) {

                window.location.href = data.redirectToUrl;
            }
        },

        error: function (err) {
            console.log(err);
        }


    });
}



function closeloginpartial() {
    document.getElementById("displayLogReg").style.display = "none";
}
//function showbadge() {
//    let cartCount = sessionStorage.getItem("cartcountS");
//    console.log(cartCount);
//}


//showbadge();

function cancelOrder(orderid) {
    $.ajax({
        url: '/Admin/DeleteOrderConfirmed',
        data: { id: orderid },
        dataType: 'json',
        success: function (data) {
            if (data.success) {
                window.location.href = data.redirectToUrl;
            }
        },

        error: function (err) {
            console.log(err);
        }


    });
}