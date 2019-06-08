using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using APIServices.Models;

namespace APIServices
{
    class Dictionary
    {
        public int Insert(string valueEsp, string valueEng)
        {
            OzHotelesEntities db = new OzHotelesEntities();

            using (System.Data.Entity.DbContextTransaction transaction = db.Database.BeginTransaction())
            {
                try
                {
                    var newIndice = new Indice
                    {
                        Eliminado = false
                    };
                    db.Indice.Add(newIndice);

                    List<Diccionario> newDictionary = new List<Diccionario>();

                    newDictionary.Add(new Diccionario {
                        IdDiccionario = newIndice.idDiccionario,
                        IdIdioma = 1,
                        Texto = valueEsp
                    });

                    newDictionary.Add(new Diccionario
                    {
                        IdDiccionario = newIndice.idDiccionario,
                        IdIdioma = 2,
                        Texto = valueEng
                    });

                    db.Diccionario.AddRange(newDictionary);
                    db.SaveChanges();
                    transaction.Commit();

                    return newIndice.idDiccionario;
                }
                catch
                {
                    transaction.Rollback();
                    return 0;
                }
                
            }
        }
    }
}
