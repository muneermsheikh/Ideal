import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { Router } from '@angular/router';
import { IEmail } from '../shared/models/email';
import { EmailParams } from '../shared/models/emailParams';
import { UsersService } from '../users/users.service';
import { EmailService } from './email.service';

@Component({
  selector: 'app-email',
  templateUrl: './email.component.html',
  styleUrls: ['./email.component.scss']
})
export class EmailComponent implements OnInit {
  @ViewChild('search', {static: false}) searchTerm: ElementRef;
  emails: IEmail[];
    // form: FormGroup;
  errors: string[];

  enquiryParams = new EmailParams();
  totalCount: number;
  sortSelected = 'asc';
  sortOptions = [
    { name: 'Ascending by category', value: 'categoryNameasc' },
    { name: 'Descending by category', value: 'categoryNameDesc' },
    { name: 'Ascending by City Name', value: 'cityNameTypeAsc' },
    { name: 'Descending by City Name', value: 'cityNameDesc' }
  ];

  constructor(private service: EmailService,
              private fb: FormBuilder,
              private router: Router
            ) { }

  ngOnInit(): void {
    this.getEmails();
  }

  editButtonClick(id: number): void {
    this.router.navigate(['/emailEdit', id]);
  }

    getEmails(useCache = false): void {
      this.service.getEmails(useCache).subscribe(response => {
      this.emails = response.data;
      this.totalCount = response.count;
      this.enquiryParams.pageNumber = response.pageIndex;
      this.enquiryParams.pageSize = response.pageSize;
    }, error => {
      console.log(error);
    });
  }

  onPageChanged(event: any): void {
    const params = this.service.getParams();
    if (params.pageNumber !== event) {
      params.pageNumber = event;
      this.service.setParams(params);
      this.getEmails(true);
    }
  }


  onSearch(): void{
    console.log(this.searchTerm.nativeElement.value);
    const params = this.service.getParams();
    params.search = this.searchTerm.nativeElement.value;
    params.pageNumber = 1;
    this.service.setParams(params);

    this.getEmails();
  }


  onReset(): any {
    this.searchTerm.nativeElement.value = '';
    this.enquiryParams = new EmailParams();
    // this.shopService.setShopParams(this.shopParams);
    this.getEmails();
  }

  onSortSelected(sort: string): any {
    const params = this.service.getParams();
    params.sort = sort;
    this.service.setParams(params);
    this.getEmails();
  }

}
