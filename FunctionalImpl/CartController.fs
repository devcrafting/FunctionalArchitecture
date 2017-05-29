module FunctionalArchitecture

open System

type AddProduct = {
    CartId: Guid
    ProductId: Guid
    Quantity: int
}

type TemporaryReservation = {
    Id: Guid
}

type ProductStock = {
    Id: Guid
    Quantity: int
}

type Cart = {
    Id: Guid
    Items: CartItem list
}
and CartItem = {
    ProductId: Guid
    Quantity: int
    Reservations: TemporaryReservation list
}

type Result<'a> = Success of 'a | Failure of string

let makeATemporaryReservation (addProduct:AddProduct) (productStock:ProductStock) = 
    let remainingQuantity = productStock.Quantity - addProduct.Quantity
    if remainingQuantity >= 0 then
        Success ({ TemporaryReservation.Id = Guid.NewGuid() }, { productStock with Quantity = remainingQuantity })
    else
        Failure "Not enough stock"

let addProductToCart (addProduct:AddProduct) (temporaryReservation:TemporaryReservation) (cart:Cart) =
    // Nothing is modified here (immutable by default)
    let cartItems, cartItem = 
        match cart.Items |> List.tryFind (fun i -> i.ProductId = addProduct.ProductId) with
        | Some item -> 
            cart.Items |> List.except [item], 
            { item with Quantity = item.Quantity + addProduct.Quantity; Reservations = temporaryReservation :: item.Reservations }
        | None -> 
            cart.Items, 
            { ProductId = addProduct.ProductId; Quantity = addProduct.Quantity; Reservations = [ temporaryReservation ] }
    if cartItem.Quantity > 99 then
        Failure "Too many items of same product"
    // ... many other rules on cart content
    else
        Success { cart with Items = cartItem :: cartItems }

let getProductStock id = 
    // Some data access...
    { Id = id; Quantity = 10 }

let saveProductStock productStock = 
    // Some data access...
    ()

let getCart id =
    // Some data access...
    { Id = id; Items = [] }

// Controller
open System.Net
open System.Net.Http

let addProductToCartController (addProduct:AddProduct) =
    getProductStock addProduct.ProductId
    |> makeATemporaryReservation addProduct
    |> function 
        | Success (tempReservation, productStock) -> 
            saveProductStock productStock
            getCart addProduct.CartId
            |> addProductToCart addProduct tempReservation
            |> function
                | Success cart -> new HttpResponseMessage(HttpStatusCode.OK)
                | Failure message -> new HttpResponseMessage(HttpStatusCode.BadRequest)
        | Failure message -> new HttpResponseMessage(HttpStatusCode.BadRequest)
