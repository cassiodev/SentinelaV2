using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sentinela.Models
{
    public static class Extensions
    {
        public static Usuario Autentica(this Usuario usuario, Contexto _Contexto)
        {
            return _Contexto.Usuario.FirstOrDefault(u => string.Equals(u.Login, usuario.Login) &&
                                                             string.Equals(u.Senha, usuario.Senha));
        }
        public static MvcHtmlString AntiForgeryTokenForAjaxPost(this HtmlHelper helper)
        {
            var antiForgeryInputTag = helper.AntiForgeryToken().ToString();
            // Above gets the following: <input name="__RequestVerificationToken" type="hidden" value="PnQE7R0MIBBAzC7SqtVvwrJpGbRvPgzWHo5dSyoSaZoabRjf9pCyzjujYBU_qKDJmwIOiPRDwBV1TNVdXFVgzAvN9_l2yt9-nf4Owif0qIDz7WRAmydVPIm6_pmJAI--wvvFQO7g0VvoFArFtAR2v6Ch1wmXCZ89v0-lNOGZLZc1" />
            var removedStart = antiForgeryInputTag.Replace(@"<input name=""__RequestVerificationToken"" type=""hidden"" value=""", "");
            var tokenValue = removedStart.Replace(@""" />", "");
            if (antiForgeryInputTag == removedStart || removedStart == tokenValue)
                throw new InvalidOperationException("Oops! The Html.AntiForgeryToken() method seems to return something I did not expect.");
            return new MvcHtmlString(string.Format(@"{0}:""{1}""", "__RequestVerificationToken", tokenValue));
        }

        public static Cardapio SetRefeicoes(this Cardapio cardapio, Contexto _Contexto, FormCollection form) 
        {

            var itensToDelete = cardapio.CardapioRefeicaoItem.Select(c => Convert.ToInt32(c.CardapioId)).ToArray();

            if (itensToDelete != null && itensToDelete.Count() > 0)
            {
                string strItensToDelete = string.Join(",", itensToDelete);
                _Contexto.Database.ExecuteSqlCommand("DELETE CardapioRefeicaoItem WHERE CardapioId in (" + strItensToDelete + ")");
            }


            cardapio.CardapioRefeicaoItem = new List<CardapioRefeicaoItem>();


            var refeicoes = _Contexto.Refeicao.Where(f => f.Ativo).ToList();

            foreach (var refeicao in refeicoes)
            {
                string key = "Refeicao-" + refeicao.RefeicaoId;

                var selectValues = form.GetValues(key);

                if (selectValues != null)
                    foreach (int item in selectValues.Select(i => Convert.ToInt32(i)))
                    {
                        cardapio.CardapioRefeicaoItem.Add(new CardapioRefeicaoItem()
                        {
                            CardapioId = cardapio.CardapioId,
                            RefeicaoId = refeicao.RefeicaoId,
                            ItemId = item
                        });
                    }
                
            }

            return cardapio;           

        }
    }
}