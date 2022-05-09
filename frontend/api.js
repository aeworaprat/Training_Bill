// unit
const end_point = "http://localhost:5252";

async function GetAllUnit()
{
    const result = await axios.get(end_point+'/Unit/GetAll',);
    const data = result.data.data;
    return data;
}

async function InsertUnti(name)
{
    let result
    await axios.post(end_point+'/Unit/InsertUnit', { 
        unit_name: name 
    }).then(function (response) {
        result = response.data;
    })
    return result;
}

async function DeleteUnit(id, name)
{
    let result
    await axios.post(end_point+'/Unit/DeleteUnit', {
        unit_id : id,
        unit_name: name
    }).then(function (response) {
        result = response.data
    })
    return result
}

async function UpdateUnit(id ,name)
{
    let result
    await axios.post(end_point+'/Unit/UpdateUnit', {
        unit_id : id,
        unit_name: name
    }).then(function (response) {
        result = response.data
    })
    return result
}

//item
async function GetAllItem()
{
    const result = await axios.get(end_point+'/Item/GetAll',);
    const data = result.data.data;
    return data;
}

async function InsertItem(name, price, unit_id)
{
    let result
    await axios.post('http://localhost:5252/Item/InsertItem', { 
        item_name : name,
        item_price : Number(price),
        item_unit_id : Number(unit_id)
    }).then(function (response) {
        result = response.data;
    })
    return result
}

async function UpdateItem(name, price, unit_id, item_id){
    let result
    await axios.post('http://localhost:5252/Item/UpdateItem', { 
        item_id : item_id,
        item_name : name,
        item_price : Number(price),
        item_unit_id : Number(unit_id)
    }).then(function (response) {
        result = response.data;
    })
    return result
}


async function DeleteItem(id, code, name)
{
    let result
    await axios.post('http://localhost:5252/Item/DeleteItem', {
        item_id : id,
        item_code : code,
        item_name : name
    }).then(function (response) {
        result = response.data;
    })
    return result
}

//receipt manage

async function GetAllReceipt()
{
    const result = await axios.get('http://localhost:5252/Receipt/GetAll',);
    const data = result.data.data;
    return data;
}

// receipt info

async function GetReceiptById(id){
    const result_receipt = await axios.get(`http://localhost:5252/Receipt/GetReceiptById?id=${id}`);
    const data_receipt = result_receipt.data
    return data_receipt
}

// receipt create
async function InsertReceipt(receipt_product_price, receipt_product_discount, receipt_discount, receipt_total_price, list){
    let result
    await axios.post('http://localhost:5252/Receipt/InsertReceipt', {
        receipt_product_price : receipt_product_price,
        receipt_product_discount : receipt_product_discount,
        receipt_discount : receipt_discount,
        receipt_total_price, receipt_total_price,
        receipt_list : list
    }).then(function (response) {
        result = response.data;
    })
    return result
}
