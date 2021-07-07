using eAgenda.Dominio.CompromissoModule;
using eAgenda.Dominio.ContatoModule;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eAgenda.Tests
{
    [TestClass]
    public class CompromissoTeste
    {


        [TestMethod]
        public void DeveEstarValido()
        {
            var contato = new Contato("José Pedro", "jose.pedro@gmail", "321654987", "JP Ltda", "Desenvolvedor");

            var compromisso = new Compromisso("validar campos", "unidade testes", "visual studio",
                              new DateTime(2022, 12, 13), new TimeSpan(3, 00, 00), new TimeSpan(4, 00, 00), contato);

            var resultadoValidacao = compromisso.Validar();


            resultadoValidacao.Should().Be("ESTA_VALIDO");
        }

        [TestMethod]
        public void DeveRetornaDataObrigatoria()
        {
            var contato = new Contato("José Pedro", "jose.pedro@gmail", "321654987", "JP Ltda", "Desenvolvedor");

            var compromisso = new Compromisso("validar campos", "unidade testes", "visual studio",
                              new DateTime(), new TimeSpan(3, 00, 00), new TimeSpan(4, 00, 00), contato);

            var resultadoValidacao = compromisso.Validar();


            resultadoValidacao.Should().Be("O campo Data é obrigatório");
        }

        [TestMethod]
        public void DeveRetornaAssuntoObrigatorio()
        {
            var contato = new Contato("José Pedro", "jose.pedro@gmail", "321654987", "JP Ltda", "Desenvolvedor");

            var compromisso = new Compromisso("", "unidade testes", "visual studio",
                              new DateTime(2020,12,03), new TimeSpan(3, 00, 00), new TimeSpan(4, 00, 00), contato);

            var resultadoValidacao = compromisso.Validar();


            resultadoValidacao.Should().Be("O campo Assunto é obrigatório");
        }


    }
}
