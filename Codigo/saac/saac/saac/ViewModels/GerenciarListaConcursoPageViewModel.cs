using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using saac.Interfaces;
using saac.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace saac.ViewModels
{
    public class GerenciarListaConcursoPageViewModel : ViewModelBase
    {
        private bool _atualizando = false;
        public bool Atualizando
        {
            get { return _atualizando; }
            set { SetProperty(ref _atualizando, value); }
        }

        private string _titulo;
        public string Titulo
        {
            get { return _titulo; }
            set { SetProperty(ref _titulo, value); }
        }

        private string _mensagem;
        public string Mensagem
        {
            get { return _mensagem; }
            set { SetProperty(ref _mensagem, value); }
        }

        private ObservableCollection<Concurso> _listaConcursos;
        public ObservableCollection<Concurso> ListaConcursos
        {
            get { return _listaConcursos; }
            set { SetProperty(ref _listaConcursos, value); }
        }

        private readonly INavigationService _navigationService;
        private readonly IAzureServiceConcurso<Concurso> _clienteConcurso;

        private DelegateCommand _opcaoCommand;
        public DelegateCommand OpcaoCommand =>
            _opcaoCommand ?? (_opcaoCommand = new DelegateCommand(Opcao));


        public GerenciarListaConcursoPageViewModel(INavigationService navigationService, IAzureServiceConcurso<Concurso> clienteConcurso) : base (navigationService)
        {
            _navigationService = navigationService;

            _clienteConcurso = clienteConcurso;

            ListaConcursos = new ObservableCollection<Concurso>();

        }

        public void AtualizarGrupos()
        {
            Atualizando = true;

            ExibirConcursos(Titulo);

            Atualizando = false;

        }

        public void ExibirConcursos(string tipo)
        {

            var dataAtual = DateTime.Now.Date;

            if (tipo.Contains("Inscrições Finalizadas"))
            {
                Inscricoesfinalizadas(dataAtual);
            }
            else
            {
                ConcursosFinalizados(dataAtual);
            }


        }

        public async void Inscricoesfinalizadas(DateTime dataAtual)
        {
            var lista = await _clienteConcurso.InscricoesFinalizadas(dataAtual);

            if (lista.Count != 0)
            {
                ListaConcursos.Clear();
                foreach (var item in lista)
                {
                    ListaConcursos.Add(item);

                }
            }
            else
            {
                Mensagem = "0";

            }

        }

        public async void ConcursosFinalizados(DateTime dataAtual)
        {
            var lista = await _clienteConcurso.ConcursosFinalizados(dataAtual);

            if (lista.Count != 0)
            {
                ListaConcursos.Clear();
                foreach (var item in lista)
                {
                    ListaConcursos.Add(item);
                }
            }
            else
            {
                Mensagem = "0";
            }

        }

        public void Opcao()
        {
            if (Titulo.Contains("Inscrições Finalizadas"))
            {
                Alterar();
            }
            else
            {
                //ConcursosFinalizados(dataAtual);
            }

        }

        public async void Alterar()
        {
            foreach (var item in ListaConcursos)
            {
                item.Visibilidade = false;
                await _clienteConcurso.AtualizarTable(item);
            }
        }


        public override void OnNavigatedTo(NavigationParameters parameters)
        {
            if (parameters.ContainsKey("titulo"))
            {
                Titulo = (string)parameters["titulo"];

                ExibirConcursos(Titulo);
            }
        }
    }
}