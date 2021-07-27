import { Component, OnInit, TemplateRef } from '@angular/core';
import {
  AbstractControl,
  FormArray,
  FormBuilder,
  FormControl,
  FormGroup,
  Validators,
} from '@angular/forms';

import { ActivatedRoute, Router } from '@angular/router';
import { Evento } from '@app/models/Evento';
import { Lote } from '@app/models/Lote';
import { EventoService } from '@app/services/evento.service';
import { LoteService } from '@app/services/lote.service';
import { environment } from '@environments/environment';

import { BsLocaleService } from 'ngx-bootstrap/datepicker';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-evento-detalhe',
  templateUrl: './evento-detalhe.component.html',
  styleUrls: ['./evento-detalhe.component.scss'],
})
export class EventoDetalheComponent implements OnInit {
  evento = {} as Evento;
  eventoId: number;
  loteAtual = { id: 0, nome: '', indice: 0 };

  form: FormGroup;
  estadoSalvar = 'post';

  modalRef = {} as BsModalRef;

  imagemURL = 'assets/upload.jpg';
  file = {} as File;

  get modoEditar(): boolean {
    return this.estadoSalvar === 'put';
  }

  get lotes(): FormArray {
    return this.form.get('lotes') as FormArray;
  }

  get f(): any {
    return this.form.controls;
  }

  get bsconfig(): any {
    return {
      isAnimated: true,
      adaptivePosition: true,
      dateInputFormat: 'DD/MM/YYYY hh:mm a',
      containerClass: 'theme-default',
      showWeekNumbers: false,
    };
  }

  get bsconfigLote(): any {
    return {
      isAnimated: true,
      adaptivePosition: true,
      dateInputFormat: 'DD/MM/YYYY',
      containerClass: 'theme-default',
      showWeekNumbers: false,
    };
  }

  constructor(
    private fb: FormBuilder,
    private localeService: BsLocaleService,
    private modalService: BsModalService,
    private activatedRouter: ActivatedRoute,
    private eventoService: EventoService,
    private loteService: LoteService,
    private spinner: NgxSpinnerService,
    private toastr: ToastrService,
    private router: Router
  ) {
    this.localeService.use('pt-br');
  }

  //#region Métodos Relacionados ao Evento
  public carregarEvento(): void {
    this.eventoId = +this.activatedRouter.snapshot.paramMap.get('id')!;
    if (this.eventoId !== null && this.eventoId !== 0) {
      this.estadoSalvar = 'put';
      this.spinner.show();
      this.eventoService
        .getEventoById(this.eventoId)
        .subscribe({
          next: (evento: Evento) => {
            this.evento = { ...evento };
            this.form.patchValue(this.evento);
            if (this.evento.imagemURL !== '') {
              this.imagemURL =
                environment.apiURL +
                'Resources/Images/' +
                this.evento.imagemURL;
            }
            this.evento.lotes.forEach((lote) =>
              this.lotes.push(this.criarLote(lote))
            );
          },
          error: (error: any) => {
            this.spinner.hide();
            this.toastr.error('Erro ao carregar o Evento', 'Erro!');
          },
        })
        .add(() => this.spinner.hide());
    }
  }

  public salvarEvento(): void {
    if (this.form.valid) {
      this.spinner.show();
      if (this.estadoSalvar === 'post') {
        this.evento = { ...this.form.value };
        this.eventoService
          .post(this.evento)
          .subscribe({
            next: (resultadoEvento: Evento) => {
              this.toastr.success('Evento salvo com sucesso', 'Sucesso');
              this.router.navigate([`eventos/detalhe/${resultadoEvento.id}`]);
            },
            error: (error: any) => {
              console.error(error);
              this.spinner.hide();
              this.toastr.error('Erro ao salvar evento', 'Erro');
            },
          })
          .add(() => this.spinner.hide());
      } else {
        this.evento = { id: this.evento.id, ...this.form.value };
        this.eventoService
          .put(this.evento)
          .subscribe({
            next: () => {
              this.toastr.success('Evento salvo com sucesso', 'Sucesso');
            },
            error: (error: any) => {
              console.error(error);
              this.spinner.hide();
              this.toastr.error('Erro ao salvar evento', 'Erro');
            },
          })
          .add(() => this.spinner.hide());
      }
    }
  }

  //#endregion

  //#region Métodos relacionado ao Lote

  public addLote(): void {
    this.lotes.push(this.criarLote({ id: 0 } as Lote));
  }

  public criarLote(lote: Lote): FormGroup {
    return this.fb.group({
      id: [lote.id, Validators.required],
      nome: [lote.nome, Validators.required],
      quantidade: [lote.quantidade, Validators.required],
      preco: [lote.preco, Validators.required],
      dataInicio: [lote.dataInicio, Validators.required],
      dataFim: [lote.dataFim, Validators.required],
    });
  }

  public salvarLotes(): void {
    this.spinner.show();

    if (this.form.controls.lotes.valid) {
      this.loteService
        .saveLote(this.eventoId, this.form.value.lotes)
        .subscribe(
          () => {
            this.toastr.success('Lotes salvos com sucesso', 'Sucesso');
          },
          (error: any) => {
            this.toastr.error('Erro ao salvar lotes', 'Erro');
            console.error(error);
          },
          () => {}
        )
        .add(() => this.spinner.hide());
    }
  }

  public removerLote(indice: number, template: TemplateRef<any>): void {
    this.loteAtual.id = this.lotes.get([indice, 'id'])?.value;
    this.loteAtual.nome = this.lotes.get([indice, 'nome'])?.value;
    this.loteAtual.indice = indice;

    if (this.loteAtual.id == 0) {
      this.lotes.removeAt(this.loteAtual.indice);
    } else {
      this.modalRef = this.modalService.show(template, { class: 'modal-sm' });
    }
  }

  public confirmDeleteLote(): void {
    this.modalRef.hide();
    this.spinner.show();
    this.loteService
      .deleteLote(this.eventoId, this.loteAtual.id)
      .subscribe(
        () => {
          this.lotes.removeAt(this.loteAtual.indice);
          this.toastr.success('Lote deletado com sucesso', 'Sucesso');
        },
        (error: any) => {
          this.toastr.error(
            `Erro ao deletar o lote: ${this.loteAtual.nome}`,
            'Erro'
          );
          console.error(error);
        }
      )
      .add(() => this.spinner.hide());
  }

  public declineDeleteLote(): void {
    this.modalRef.hide();
  }

  public retornaTituloLote(nome: string): string {
    return nome == null || !/\S/.test(nome) ? 'Nome do Lote' : nome;
  }

  //#endregion

  ngOnInit(): void {
    this.validation();
    this.carregarEvento();
  }

  public validation(): void {
    this.form = this.fb.group({
      tema: [
        '',
        [
          Validators.required,
          Validators.minLength(4),
          Validators.maxLength(50),
        ],
      ],
      local: ['', Validators.required],
      dataEvento: ['', Validators.required],
      qtdPessoas: ['', [Validators.required, Validators.max(120000)]],
      telefone: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      imagemURL: [''],
      lotes: this.fb.array([]),
    });
  }

  public resetForm(): void {
    console.log('Resetou');
    this.form.reset();
  }

  cssValidator(campoForm: FormControl | AbstractControl | null): any {
    return { 'is-invalid': campoForm?.errors && campoForm?.touched };
  }

  //#region Métodos relacionados ao upload da Imagem

  public onFileChange(ev: any): void {
    const reader = new FileReader();

    //reader.onload = (event: any) => this.imagemURL = event.target.result;
    reader.onload = () => {
      this.imagemURL = reader.result as string;
    };

    this.file = ev.target.files[0];
    reader.readAsDataURL(this.file);

    this.uploadImagem();
  }

  public uploadImagem(): void {
    this.spinner.show();
    this.eventoService
      .postUpload(this.eventoId, this.file)
      .subscribe(
        () => {
          this.router.navigate([`eventos/detalhe/${this.eventoId}`]);
          this.toastr.success('Imagem atualizada com sucesso', 'Sucesso');
        },
        (error: any) => {
          console.error(error);
          this.toastr.error(`Erro ao carregar imagem`, 'Erro');
        }
      )
      .add(() => this.spinner.hide());
  }

  //#endregion
}
