using PortaleFornitori.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PortaleFornitori.Helpers
{
    public class CookieHelper
    {
        public static dynamic GetUserData(Utente utente)
        {
            if (utente.Fornitore == null)
            {
                return GetUtenteUserData(utente);
            }
            else
            {
                return GetFornitoreUserData(utente);
            }
        }
        private static dynamic GetUtenteUserData(Utente utente)
        {
            return new
            {
                Cognome = utente.Cognome,
                Email = utente.Email,
                IdRuolo = utente.IdRuolo,
                IdUser = utente.IdUser,
                Nome = utente.Nome,
                Password = utente.Password,
                Ruolo = new
                {
                    DescrizioneRuolo = utente.Ruolo.DescrizioneRuolo,
                    IdRuolo = utente.Ruolo.IdRuolo
                }
            };
        }

        private static dynamic GetFornitoreUserData(Utente utente) {
            return new
            {
                Cognome = utente.Cognome,
                Email = utente.Email,
                Fornitore = new
                {
                    IdFornitore = utente.Fornitore.IdFornitore,
                    RagioneSociale = utente.Fornitore.RagioneSociale,
                    Indirizzo = utente.Fornitore.Indirizzo,
                    Citta = utente.Fornitore.Citta,
                    Telefono = utente.Fornitore.Telefono,
                },
                IdFornitore = utente.IdFornitore,
                IdRuolo = utente.IdRuolo,
                IdUser = utente.IdUser,
                Nome = utente.Nome,
                Password = utente.Password,
                Ruolo = new
                {
                    DescrizioneRuolo = utente.Ruolo.DescrizioneRuolo,
                    IdRuolo = utente.Ruolo.IdRuolo,
                }
            };
        }
    }
}