namespace E_commerce_domain.shared;

public enum ErrorType
{
    None = 0, //there is no error 200
    Validation = 1, //the data i get is wrong, bad request 400
    NotFound = 2,  //404
    Conflict = 3, //this data is already exists 409
    Unauthorized = 4, //401
    Forbidden = 5, //he is Unauthorized already but, required role big than his role 403 
    Failure = 6 //500
}