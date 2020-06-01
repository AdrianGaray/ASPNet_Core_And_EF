using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace WebNetCore3.Library
{
    public class ExportData : Controller
    {
        private List<String[]> _listData;
        private String _fileName, _table;
        private String[] _titles;
        private IWebHostEnvironment _hostingEnvironment;

        public ExportData(IWebHostEnvironment hostingEnvironment, List<String[]> listData
            , String[] titles, String fileName, String table)
        {
            _table = table;
            _fileName = fileName;
            _titles = titles;
            _listData = listData;
            _hostingEnvironment = hostingEnvironment;
        }

        public async Task<IActionResult> ExportExcelAsync()
        {
            // para poder obtener la direccion de nuestro proyecto o de nuestra apliacacion
            string sWebRootFolder = _hostingEnvironment.WebRootPath;
            
            // almacena en memoria una informacion
            var memory = new MemoryStream();

            // la clase FileStream  es para obtener un archivo y guardarlo en memoria
            using (var fs = new FileStream(Path.Combine(sWebRootFolder, _fileName), FileMode.Create, FileAccess.Write))
            {
                // fs guarda la direccion de un archivo  que vamos a obtener y que vamos a crear
                // instalamos el paquete. y tenemso acceso a la interface workbook
                IWorkbook workbook;
                workbook = new XSSFWorkbook();

                // creamos una hoja en el archivo de tipo excel
                // _table contiene el nombre de la hoja de ecxel
                ISheet excelSheet = workbook.CreateSheet(_table);
                
                // creamos una fila
                IRow row = excelSheet.CreateRow(0);

                // ahora creamos columnas y esas columnas vamos a crearles titulos
                for (int i = 0; i < _titles.Length; i++)
                {
                    // se crea columnas en la fila 0
                    row.CreateCell(i).SetCellValue(_titles[i]);
                }

                // se inicializa en uno porque ahora vamos a crear filas
                int count = 1;

                for (int i = 0; i < _listData.Count; i++)
                {
                    row = excelSheet.CreateRow(count);
                    var list = _listData[i];

                    for (int j = 0; j < _titles.Length; j++)
                    {
                        row.CreateCell(j).SetCellValue(list[j]);
                    }
                    count++;
                }
                // escribimos en nuestra hoja de excel.
                workbook.Write(fs);
            }

            // sWebRootFolder = combinamos la direccin de nuestro proyecto
            // _fileName = proporcionamos el nombre
            // abrimos el archivo FileMode.Open
            using (var stream = new FileStream(Path.Combine(sWebRootFolder, _fileName), FileMode.Open))
            {
                // vamos a obtener el archivo y lo guardamos en stream
                // la informacion que vamos a obtner del archivo stream lo copiamos a  memory
                // obtenemos el archivo que vamos almacenar en memory para poder obtenelo nuevamente y lo almacenamos en memory
                await stream.CopyToAsync(memory);
            }

            memory.Position = 0;

            // descargamos el archivo excel, con el nombre del excel
            return File(memory,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                _fileName);
        }
    }
}
