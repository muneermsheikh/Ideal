import { Component, ElementRef, Input, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, FormGroupDirective, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { IClient } from 'src/app/shared/models/client';
import { ClientParams } from 'src/app/shared/models/clientParams';
import { IIndustryType } from 'src/app/shared/models/industryType';
import { ClientsService } from './clients.service';

@Component({
  selector: 'app-clients',
  templateUrl: './clients.component.html',
  styleUrls: ['./clients.component.scss']
})
export class ClientsComponent implements OnInit {
  mode: string;   // client or associate or vendors - read from activatedRoute
  formTitle: string;

  @ViewChild('search', {static: false}) searchTerm: ElementRef;
  @Input() clientType: number;
  indTypes: IIndustryType[];
  form: FormGroup;
  clients: IClient[] = [];

  clientParams = new ClientParams();
  totalCount: number;
  sortSelected = 'asc';
  sortOptions = [
    { name: 'Ascending by name', value: 'namesAsc' },
    { name: 'Descending by name', value: 'namesDesc' },
    { name: 'Ascending by City', value: 'cityAsc' },
    { name: 'Descending by City', value: 'cityDesc' }
  ];

  constructor(private fb: FormBuilder, private service: ClientsService,
              private router: Router, private activatedRout: ActivatedRoute) { }

  ngOnInit(): void {
    this.activatedRout.paramMap.subscribe(params => {
      this.mode = params.get('mode');
    });

    if (this.mode === 'client') {
      this.formTitle = 'Clients';
    } else if (this.mode === 'associate') {
      this.formTitle = 'Associates';
    } else {
      this.formTitle = 'Vendors';
    }
    this.getClients();

    this.createForm();
    console.log(this.formTitle + this.clients);
  }


  createForm(): void {
    this.form = this.fb.group
    ({
      customerType: [this.formTitle, Validators.required],
      customerName: [null, [Validators.required]],
      knownAs: [null, [Validators.required]],
      city: [null, Validators.required],
      country: [null, Validators.required],
      email: [null, [Validators.required]],
      companyUrl: [null],
      customerStatus: [null]
    });
  }

  onPageChanged(event: any): void {
    const params = this.service.getClientParams();
    if (params.pageNumber !== event) {
      params.pageNumber = event;
      this.service.setClientParams(params);
      this.getClients(true);
    }
  }

  editButtonClick(id: number): void {
    this.router.navigate(['/clientEdit', id]);
    /* if (this.formTitle === 'Clients') {
      this.router.navigate(['/clientEdit', id]);
    } else if (this.formTitle === 'Associates') {
      this.router.navigate(['/clientEdit', id, this.formTitle])
    }
    */
    // this.router.navigate(['/clientEdit', id, this.formTitle]);
  }


  onSearch(): void{
    // console.log(this.searchTerm.nativeElement.value);
    const params = this.service.getClientParams();
    params.search = this.searchTerm.nativeElement.value;
    params.pageNumber = 1;
    this.service.setClientParams(params);

    this.getClients();
  }


  onReset(): any {
    this.searchTerm.nativeElement.value = '';
    this.clientParams = new ClientParams();

    this.getClients();
  }

  onSortSelected(sort: string): any {
    const params = this.service.getClientParams();
    params.sort = sort;
    this.service.setClientParams(params);
    this.getClients();
  }

  // gets


  getClients(useCache = false): void  {
    this.service.getClients(useCache).subscribe(response => {
    this.clients = response.data;
    this.totalCount = response.count;
    this.clientParams.pageNumber = response.pageIndex;
    this.clientParams.pageSize = response.pageSize;
  }, error => {
    console.log(error);
  });
}



}
