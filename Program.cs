using SistemaImobiliaria.Services;
using SistemaImobiliaria.UI;

Console.OutputEncoding = System.Text.Encoding.UTF8;

var jsonService = new JsonService("Data");
var imobiliariaService = new ImobiliariaService(jsonService);

Menu.Iniciar(imobiliariaService);
