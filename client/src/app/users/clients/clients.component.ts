import { Component, ElementRef, Input, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, FormGroupDirective, Validators } from '@angular/forms';
import { IClient } from 'src/app/shared/models/client';
import { ClientParams } from 'src/app/shared/models/clientParams';
import { ClientsService } from './clients.service';

@Component({
  selector: 'app-clients',
  templateUrl: './clients.component.html',
  styleUrls: ['./clients.component.scss']
})
export class ClientsComponent implements OnInit {
  @ViewChild('search', {static: false}) searchTerm: ElementRef;
  @Input() clientType: number;
  params = new ClientParams();
  totalCount: number;
  form: FormGroup;
  clients: IClient[] = [];

  constructor(private fb: FormBuilder, private service: ClientsService) { }

  ngOnInit(): void {

    this.createForm();
  }


  createForm() {
    this.form = this.fb.group
    ({
      customerName: [null, [Validators.required]],
      knownAs: [null, [Validators.required]],
      cityName: [null, [Validators.required]],
      introducedBy: [null],
      email: [null, [Validators.required]],
      mobile: [null, [Validators.required]],
      phone: [null],
      companyUrl: [null],
      description: [null],
      customerAddressId: [null],
      addedOn: [null],
    });
  }

  getClients(useCache = false)  {
    console.log('params = ' + this.params.pageNumber + ' ' + this.params.pageSize);
    this.service.getClients(useCache).subscribe(response => {
    this.clients = response.data;
    this.totalCount = response.count;
    this.params.pageNumber = response.pageIndex;
    this.params.pageSize = response.pageSize;
  }, error => {
    console.log(error);
  });
}
}
