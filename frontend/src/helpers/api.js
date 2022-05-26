// unit
import axios from 'axios'

const end_point = "http://localhost:5252";

export async function GetAllUnit()
{
    const result = await axios.get(end_point+'/Unit/GetAll',);
    const data = result.data.data;
    return data;
}

export async function InsertUnit(name)
{
    const result = await axios.post(end_point+'/Unit/InsertUnit', { 
        unit_name: name 
    }).then(function (response) {
        return response.data
    })
    return result;
}

export async function DeleteUnit(id)
{
    const result = await axios.post(end_point+'/Unit/DeleteUnit', {
        unit_id : id,
    }).then(function (response) {
        return response.data
    })
    return result
}

export async function UpdateUnit(id ,name)
{
    const result = await axios.post(end_point+'/Unit/UpdateUnit', {
        unit_id : id,
        unit_name: name
    }).then(function (response) {
        return response.data
    })
    return result
}

//item
export async function GetAllItem()
{
    const result = await axios.get(end_point+'/Item/GetAll',);
    const data = result.data.data;
    return data;
}

export async function InsertItem(name, price, unit_id)
{
    const result = await axios.post(end_point+'/Item/InsertItem', { 
        item_name : name,
        item_price : Number(price),
        item_unit_id : Number(unit_id)
    }).then(function (response) {
        return response.data;
    })
    return result
}

export async function UpdateItem(name, price, unit_id, item_id){
    const result = await axios.post(end_point+'/Item/UpdateItem', { 
        item_id : item_id,
        item_name : name,
        item_price : Number(price),
        item_unit_id : Number(unit_id)
    }).then(function (response) {
        return response.data;
    })
    return result
}


export async function DeleteItem(id)
{
    const result = await axios.post(end_point+'/Item/DeleteItem', {
        item_id : id
    }).then(function (response) {
        return response.data;
    })
    return result;
}

//receipt manage

export async function GetAllReceipt()
{
    const result = await axios.get(end_point+'/Receipt/GetAll',);
    const data = result.data.data;
    return data;
}

// receipt info

export async function GetReceiptById(id){
    const result_receipt = await axios.get(end_point+`/Receipt/GetReceiptById?id=${id}`);
    const data_receipt = result_receipt.data
    return data_receipt
}

// receipt create
export async function InsertReceipt(receipt_product_price, receipt_product_discount, receipt_discount, receipt_total_price, list){
    const result = await axios.post(end_point+'/Receipt/InsertReceipt', {
        receipt_product_price : receipt_product_price,
        receipt_product_discount : receipt_product_discount,
        receipt_discount : receipt_discount,
        receipt_total_price, receipt_total_price,
        receipt_list : list
    }).then(function (response) {
        return response.data;
    })
    return result
}

export async function GetReceiptFilterDate(start, end){
    const result = await axios.get(end_point+`/Receipt/GetReceiptFilterDate?startDate=${start}&endDate=${end}`);
    const data = result.data;
    return data;
}

// export async function GetReceiptFilterDate(start ,end){
//     const result = await axios.get(end_point+`/Receipt/GetReceiptFilterDate?startDate=${start}&endDate=${end}`);
//     const data = result.data;
//     return data;
// }
