import { BuiltinVar } from '@angular/compiler';
import { isNull } from '@angular/compiler/src/output/output_ast';
import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { Router } from '@angular/router';
import { isNullOrUndefined } from 'util';
import { EmailService } from '../email/email.service';
import { IEmail } from '../shared/models/email';
import { IEnquiryForClient } from '../shared/models/enquiryForClient';
import { EnquiryParams } from '../shared/models/enquiryParams';
import { ClientsService } from '../users/clients/clients.service';
import { UsersService } from '../users/users.service';
import { OrdersService } from './orders.service';

@Component({
  selector: 'app-orders',
  templateUrl: './orders.component.html',
  styleUrls: ['./orders.component.scss']
})
export class OrdersComponent implements OnInit {

  @ViewChild('search', {static: false}) searchTerm: ElementRef;
  enquiries: IEnquiryForClient[];
  emailModel: IEmail;
    // form: FormGroup;
  errors: string[];

  enquiryParams = new EnquiryParams();
  totalCount: number;
  sortSelected = 'asc';
  sortOptions = [
    { name: 'Ascending by category', value: 'categoryNameasc' },
    { name: 'Descending by category', value: 'categoryNameDesc' },
    { name: 'Ascending by City Name', value: 'cityNameTypeAsc' },
    { name: 'Descending by City Name', value: 'cityNameDesc' }
  ];

  constructor(private service: OrdersService,
              private router: Router,
              private mailService: EmailService,
              private custService: ClientsService) { }

  ngOnInit(): void {
    // this.getProfessions();
    this.getEnquiries();
  }

  editButtonClick(enquiryId: number): void {
    this.router.navigate(['/enquiryEdit', enquiryId]);
  }

  getEnquiries(useCache = false): void {
      this.service.getEnquiries(useCache).subscribe(response => {
      this.enquiries = response.data;
      this.totalCount = response.count;
      this.enquiryParams.pageNumber = response.pageIndex;
      this.enquiryParams.pageSize = response.pageSize;
    }, error => {
      console.log(error);
    });
  }

  onPageChanged(event: any): void {
    const params = this.service.getEnquiryParams();
    if (params.pageNumber !== event) {
      params.pageNumber = event;
      this.service.setEnquiryParams(params);
      this.getEnquiries(true);
    }
  }

  onSearch(): void{
    console.log(this.searchTerm.nativeElement.value);
    const params = this.service.getEnquiryParams();
    params.search = this.searchTerm.nativeElement.value;
    params.pageNumber = 1;
    this.service.setEnquiryParams(params);

    this.getEnquiries();
  }


  onReset(): any {
    this.searchTerm.nativeElement.value = '';
    this.enquiryParams = new EnquiryParams();
    // this.shopService.setShopParams(this.shopParams);
    this.getEnquiries();
  }

  onSortSelected(sort: string): any {
    const params = this.service.getEnquiryParams();
    params.sort = sort;
    this.service.setEnquiryParams(params);
    this.getEnquiries();
  }

  generateAcknowledgementMail(id: number): any {

    const enquiry = this.service.getEnquiry(id);

  }



}
