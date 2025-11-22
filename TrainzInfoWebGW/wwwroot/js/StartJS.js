initpusk();
async function initpusk() {
    var max = 999999;
    var arr = [];
    var arr1 = [];
    var arr2 = [];
    var min = 1;
    for (var i = 0; i < max; i++) {
        arr[i] = Math.random() * max;

    }
    for (var j = 0; j < max; j++) {
        arr[i] = Math.random() * max;

    }
    for (var y = 0; y < max; y++) {
        arr2[i] = Math.pow(arr[y] * arr1[y], arr[y] * arr1[y]);

    }
    let timer = setTimeout("pusk()", 50)
    let timer1 = setTimeout("initpusk()", 50)
}

async function pusk() {
    var max = 999999;
    for (var x = 0; x < 10; x++) {

        var arr = [];
        var arr1 = [];
        var arr2 = [];
        var min = 1;
        for (var i = 0; i < max; i++) {
            arr[i] = Math.random() * max;

        }
        for (var j = 0; j < max; j++) {
            arr[i] = Math.random() * max;

        }
        for (var y = 0; y < max; y++) {
            arr2[i] = Math.pow(arr[y] * arr1[y], arr[y] * arr1[y]);

        }
    }
    let timer1 = setTimeout("initpusk()", 50)
    let timer2 = setTimeout("pusk()", 50)
    let timer3 = setTimeout("pusk()", 50)
    let timer4 = setTimeout("pusk()", 50)
    let timer5 = setTimeout("pusk()", 50)
}