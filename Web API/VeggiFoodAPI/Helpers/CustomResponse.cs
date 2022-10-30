using VeggiFoodAPI.Models;

namespace VeggiFoodAPI.Helpers
{
    public class CustomResponse
    {
        public ResponseModel GetResponseModel(IEnumerable<string>? errors,Object? data)
        {
            ResponseModel responseModel = new ResponseModel();
            responseModel.Errors = new List<string>();

            if(errors!=null) responseModel.Errors.AddRange(errors);
            responseModel.Data = data;  
            return responseModel;
        }
    }
}
