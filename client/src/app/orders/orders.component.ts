import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { Router } from '@angular/router';
import { ClientParams } from '../shared/models/clientParams';
import { IEnquiry } from '../shared/models/enquiry';
import { EnquiryParams } from '../shared/models/enquiryParams';
import { IProfession } from '../shared/models/profession';
import { UsersService } from '../users/users.service';
import { OrdersService } from './orders.service';

@Component({
  selector: 'app-orders',
  templateUrl: './orders.component.html',
  styleUrls: ['./orders.component.scss']
})
export class OrdersComponent implements OnInit {

  @ViewChild('search', {static: false}) searchTerm: ElementRef;
  enquiries: IEnquiry[];
  professions: IProfession[];
  form: FormGroup;
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
              private fb: FormBuilder,
              private router: Router,
              private userService: UsersService) { }

  ngOnInit(): void {
    this.getProfessions();
    this.getEnquiries();
  }

  editButtonClick(enquiryId: number): void {
    this.router.navigate(['/enquiryEdit', enquiryId]);
  }

  getProfessions(): any {
    this.userService.getProfessions().subscribe(response =>
      { this.professions = response; }
      , error => {
        console.log(error); });
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
/*
  createForm(): void {
    this.form = this.fb.group({
      applicationNo: [null],
      applicationDated: [null],
      gender: [''],
      fullName: [''],
      // addedOn: [''],
      name: [null],
      categoryName: [null],
      passportNo: [null],
      aadharNo: [null],
      mobileNo: [null],
      email: [null]
    });
  }
*/
  onPageChanged(event: any): void {
    const params = this.service.getEnquiryParams();
    if (params.pageNumber !== event) {
      params.pageNumber = event;
      this.service.setEnquiryParams(params);
      this.getEnquiries(true);
    }
  }

/*
  onPageChanged(event: any): void {
    const params = this.cvService.getCandParams();
    this.candParams.pageNumber = event;
    if (params.pageNumber !== event) {
      params.pageNumber = event;
      this.cvService.setCandParams(params);
      this.getCandidates();
    }
  }
*/

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

}
