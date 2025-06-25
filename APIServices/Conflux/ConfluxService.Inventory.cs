using System;
using System.Data;
using System.Linq;
using System.Net;
using System.Collections.Generic;
using System.Xml.Linq;
using APIServices.Conflux.Models.Inventory.Response;
using Portal.Hotel.Common.Data;
using Portal.General.Common.Data;
using Portal.General.Facade;
using Portal.Hotel.Facade;

namespace APIServices.Conflux
{
    public partial class ConfluxService
    {
        public InventoryResponse UpdateInventory(List<XDocument> documents, string endpoint)
        {
            InventoryResponse res = new InventoryResponse();
            try
            {
                foreach (XDocument document in documents)
                {
                    InventoryHttpResponse inventoryHttpResponse = new InventoryHttpResponse();

                    var httpReq = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(endpoint);

                    httpReq.Method = "POST";

                    byte[] bytes = System.Text.Encoding.ASCII.GetBytes(document.ToString());
                    httpReq.ContentType = "application/xml; encoding='utf-8'";
                    httpReq.ContentLength = bytes.Length;

                    using (System.IO.Stream requestStream = httpReq.GetRequestStream())
                    {
                        requestStream.Write(bytes, 0, bytes.Length);
                    }

                    using (HttpWebResponse response = (HttpWebResponse)httpReq.GetResponse())
                    {
                        string responseText = string.Empty;

                        using (System.IO.Stream responseStream = response.GetResponseStream())
                        {
                            using (System.IO.StreamReader reader = new System.IO.StreamReader(responseStream))
                            {
                                responseText = reader.ReadToEnd();
                            }
                        }

                        if (response.StatusCode != HttpStatusCode.OK)
                        {
                            inventoryHttpResponse.IsSuccess = false;
                        }
                        else if (validStatusCodes.Contains(response.StatusCode))
                        {
                            inventoryHttpResponse.IsSuccess = true;
                        }

                        inventoryHttpResponse.Xml = responseText;
                        inventoryHttpResponse.XmlRequest = document.ToString();

                        res.InventoryHttpResponseList.Add(inventoryHttpResponse);

                        //Espera 1 segundo antes de mandar el siguiente request
                        System.Threading.Thread.Sleep(TimeSpan.FromSeconds(1));
                    }
                }

                res.IsSuccess = true;
            }
            catch (Exception ex)
            {
                res.IsSuccess = false;
                res.Error = new KeyValuePair<string, string>("448", ex.Message);
                var errorsElement = new System.Xml.Linq.XElement("Errors");
                var errorElementProperty = new System.Xml.Linq.XElement("Error");
                errorElementProperty.Add(new System.Xml.Linq.XAttribute("Type", "3"), new System.Xml.Linq.XAttribute("Code", "448"), new System.Xml.Linq.XText(ex.Message));
                errorsElement.Add(errorElementProperty);
                res.Xml = errorsElement.ToString();
            }

            return res;
        }

        public RoomsInventoryData GetInventoryData(int[] roomsIdList, DateTime? startDate, DateTime? endDate) 
        {
            RoomsInventoryData ds = new RoomsInventoryData();
            ds.Tables[0].Columns.Add("RoomCode", typeof(string));

            RoomsInventoryData dsActualInventory;

            for (int i = 0; i < roomsIdList.Length; i++)
            {
                 dsActualInventory = new RoomsInventoryFacade().getInventoryByDate_Data(roomsIdList[i], startDate.Value.Date, endDate.Value.Date);

                foreach (DataRow row in dsActualInventory.Tables[0].Rows)
                {
                    DataRow dr = ds.Tables[RoomsInventoryData.TBL_ROOMS_INVENTORY].NewRow();

                    dr[RoomsInventoryData.FLD_DATE] = row[RoomsInventoryData.FLD_DATE];
                    dr[RoomsInventoryData.FLD_STARTDATE] = row[RoomsInventoryData.FLD_DATE];
                    dr[RoomsInventoryData.FLD_ENDDATE] = row[RoomsInventoryData.FLD_DATE];
                    dr[RoomsInventoryData.FLD_ID_ROOM_HOTEL] = roomsIdList[i];
                    dr[RoomsInventoryData.FLD_NUMBER_ROOMS] = row[RoomsInventoryData.FLD_NUMBER_ROOMS];
                    dr[RoomsInventoryData.FLD_STATUS] = 0;
                    dr[RoomsInventoryData.FLD_NUMBER_AVAILABILITY] = row[RoomsInventoryData.FLD_NUMBER_AVAILABILITY];
                    dr["RoomCode"] = row["CodigoHabitacion"];

                    ds.Tables[RoomsInventoryData.TBL_ROOMS_INVENTORY].Rows.Add(dr);
                    dr.AcceptChanges();
                    dr[RoomsInventoryData.FLD_STATUS] = dr[RoomsInventoryData.FLD_STATUS];
                }
                
            }

            return ds;

        }

        public int[] LoadRoomsByIdHotel(int idHotel, int lang = 1)
        {
            List<int> roomsIdList = new List<int>();
            RoomsHotelData ds = new RoomFacade().getRooms(idHotel, lang);

            var roomsTable = ds.Tables[RoomsHotelData.TBL_ROOM_HOTEL];

            foreach (DataRow row in roomsTable.Rows)
            {
                string code = row.ItemArray[0].ToString();
                roomsIdList.Add(Convert.ToInt32(code));

            }

            return roomsIdList.ToArray();
        }

    }
}
