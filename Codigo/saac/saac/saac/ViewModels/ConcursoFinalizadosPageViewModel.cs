﻿using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using saac.Interfaces;
using saac.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace saac.ViewModels
{
	public class ConcursoFinalizadosPageViewModel : ViewModelBase
	{
        #region Propriedades
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
        private readonly IPageDialogService _dialogService;

        private readonly IAzureServiceGroup<Grupo> _clienteGrupo;
        private readonly IAzureServiceConcurso<Concurso> _clienteConcurso;
        private readonly IAzureServicePrefConcurso<PreferenciaConcurso> _clientePreferencia;
        private readonly IAzureServiceAuxConcursoGrupo<AuxConcursoGrupo> _clienteAuxConcurso;
        private readonly IAzureServicePublication<Publicacao> _clientePublicaco;
        private readonly IAzureServiceComment<Comentario> _clienteComentario;
        private readonly IAzureServiceAux<Auxiliar> _clienteAuxiliar;

        private DelegateCommand _alterarCommand;
        public DelegateCommand AlterarCommand =>
            _alterarCommand ?? (_alterarCommand = new DelegateCommand(Alterar));

        #endregion

        #region Construtor
        public ConcursoFinalizadosPageViewModel(INavigationService navigationService, IPageDialogService dialogService, IAzureServiceConcurso<Concurso> clienteConcurso,
            IAzureServiceGroup<Grupo> clienteGrupo, IAzureServicePrefConcurso<PreferenciaConcurso> clientePreferencia, IAzureServiceAuxConcursoGrupo<AuxConcursoGrupo> clienteAuxConcurso,
            IAzureServicePublication<Publicacao> clientePublicaco, IAzureServiceComment<Comentario> clienteComentario, IAzureServiceAux<Auxiliar> clienteAuxiliar) : base(navigationService)
        {
            _navigationService = navigationService;
            _dialogService = dialogService;

            _clienteGrupo = clienteGrupo;
            _clienteConcurso = clienteConcurso;
            _clientePreferencia = clientePreferencia;
            _clienteAuxConcurso = clienteAuxConcurso;
            _clientePublicaco = clientePublicaco;
            _clienteComentario = clienteComentario;
            _clienteAuxiliar = clienteAuxiliar;

            ListaConcursos = new ObservableCollection<Concurso>();

        }
        #endregion

        #region Métodos
        public void AtualizarConcursos()
        {
            Atualizando = true;

            ConcursosFinalizados();

            Atualizando = false;

        }

        public async void ConcursosFinalizados()
        {
            var dataAtual = DateTime.Now.Date;

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

        public async void Alterar()
        {
            foreach (var itemConcurso in ListaConcursos)
            {
                var Preferencia = await _clientePreferencia.ConcursoPreferencia(itemConcurso.Id);
                var ListaAux = await _clienteAuxConcurso.ListaGruposConcursos(itemConcurso.Id);
                var AuxConcurso = await _clienteAuxConcurso.GruposConcursos(itemConcurso.Id);

                await RemoverGrupo(AuxConcurso);
                
                foreach (var itemListaAux in ListaAux)
                {
                    await _clienteAuxConcurso.RemoverTable(itemListaAux);
                }
               
                await _clientePreferencia.RemoverTable(Preferencia);
                await _clienteConcurso.RemoverTable(itemConcurso);
            }

            await _dialogService.DisplayAlertAsync("Alterados", "Os concursos foram alterados", "Ok");
            await _navigationService.GoBackAsync();
        }

        public async Task RemoverGrupo(List<string> AuxConcurso)
        {
            var Grupos = await _clienteGrupo.MeusGrupos(AuxConcurso);

            foreach (var itemGrupo in Grupos)
            {
                var Publicacoes = await _clientePublicaco.Publicacoes(itemGrupo.Id);

                foreach (var itemPublicaco in Publicacoes)
                {
                    var Comentarios = await _clienteComentario.Comentarios(itemPublicaco.Id);

                    foreach (var itemComentario in Comentarios)
                    {
                        await _clienteComentario.RemoverTable(itemComentario);
                    }

                    await _clientePublicaco.RemoverTable(itemPublicaco);
                }

                var Aux = await _clienteAuxiliar.SeguidoresGrupo(itemGrupo.Id);

                foreach (var itemAux in Aux)
                {
                    await _clienteAuxiliar.RemoverTable(itemAux);

                }

                await _clienteGrupo.RemoverTable(itemGrupo);
            }

        }


        public override void OnNavigatedTo(NavigationParameters parameters)
        {
            ConcursosFinalizados();
        }

        #endregion

    }
}