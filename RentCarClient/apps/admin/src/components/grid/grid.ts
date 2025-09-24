import { httpResource } from '@angular/common/http';
import { AfterViewInit, ChangeDetectionStrategy, Component, computed, contentChild, contentChildren, inject, input, linkedSignal, signal, TemplateRef, ViewEncapsulation } from '@angular/core';
import { FlexiGridColumnComponent, FlexiGridModule, FlexiGridService, StateModel } from 'flexi-grid';
import { ODataModel } from '../../models/odata.model';
import { RouterLink } from '@angular/router';
import { FlexiToastService } from 'flexi-toast';
import { HttpService } from '../../services/http';
import { BreadcrumbModel, BreadcrumbService } from '../../services/breadcrumb';
import { NgTemplateOutlet } from '@angular/common';

@Component({
  selector: 'grid',
  imports: [
    FlexiGridModule,
    RouterLink,
    NgTemplateOutlet
  ],
  templateUrl: './grid.html',
  encapsulation: ViewEncapsulation.None,
  changeDetection: ChangeDetectionStrategy.OnPush
})
export default class Grid implements AfterViewInit {
  readonly pageTitle = input.required<string>();
  readonly endpoint = input.required<string>();
  readonly showAudit = input<boolean>(true);
  readonly addUrl = input.required<string>();
  readonly editUrl = input.required<string>();
  readonly detailUrl = input.required<string>();
  readonly deleteEndpoint = input.required<string>();
  readonly breadcrumbs = input.required<BreadcrumbModel[]>();
  readonly commandColumnWidth = input<string>("150px");
  readonly showIndex = input<boolean>(false);

  readonly columns = contentChildren(FlexiGridColumnComponent, {descendants: true});
  readonly commandTemplateRef = contentChild<TemplateRef<any>>("commandTemplate");
  readonly columnCommandTemplateRef = contentChild<TemplateRef<any>>('columnCommandTemplate');

  readonly state = signal<StateModel>(new StateModel());
  readonly result = httpResource<ODataModel<any>>(() => {
    let enpoint = this.endpoint() + '?$count=true';
    const part = this.#grid.getODataEndpoint(this.state());
    enpoint += `&${part}`;

    return enpoint;
  });
  readonly data = computed(() => this.result.value()?.value ?? []);
  readonly total = computed(() => this.result.value()?.['@odata.count'] ?? 0);
  readonly loading = computed(() => this.result.isLoading());

  readonly #breadcrumb = inject(BreadcrumbService);
  readonly #grid = inject(FlexiGridService);
  readonly #toast = inject(FlexiToastService);
  readonly #http = inject(HttpService);

  ngAfterViewInit(): void {
    this.#breadcrumb.reset(this.breadcrumbs());
  }

  dataStateChange(state: StateModel){
    this.state.set(state);
  }

  delete(id: string){
    this.#toast.showSwal('Sil?', 'Kaydı silmek istiyor musunuz?','Sil', () => {
      this.#http.delete(`${this.deleteEndpoint()}/${id}`,res => {
        //toast ile mesaj göster
        this.result.reload();
      });
    })
  }
}