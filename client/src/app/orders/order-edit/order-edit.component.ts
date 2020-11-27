import { Component, OnInit } from '@angular/core';
import { FormArray, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { EmployeesService } from 'src/app/employees/employees.service';
import { IClient, IClientOfficial } from 'src/app/shared/models/client';
import { IEmployee } from 'src/app/shared/models/employee';
import { IEnquiry, IEnquiryItem } from 'src/app/shared/models/enquiry';
import { IProfession } from 'src/app/shared/models/profession';
import { UsersService } from 'src/app/users/users.service';
import { OrdersService } from '../orders.service';

@Component({
  selector: 'app-order-edit',
  templateUrl: './order-edit.component.html',
  styleUrls: ['./order-edit.component.scss']
})
export class OrderEditComponent implements OnInit {
  createForm: FormGroup;
  enquiry: IEnquiry;
  pageTitle: string;
  professions: IProfession[];
  employees: IEmployee[];
  allOfficials: IClientOfficial[];
  officials: IClientOfficial[];
  customers: IClient[];
  clientIdSelected: number;

  formErrors = {
  };

  constructor(private fb: FormBuilder,
              private service: OrdersService,
              private userService: UsersService,
              private empService: EmployeesService,
              private activatedRoute: ActivatedRoute,
              private route: Router)
  {
  }

  ngOnInit(): void {
    this.createForm = this.fb.group({
      id: [0],
      customerId: [null, Validators.required],
      enquiryRef: [null],
      enquiryNo: [null],
      enquiryDate: [null, Validators.required],
      basketId: [null],
      completeBy: [null , Validators.required],
      readyToReview: [null],
      reviewStatus: [null],
      reviewedById:  [null],
      reviewedOn:  [null],
      enquiryStatus:  [null],
      projectManagerId:  [null, Validators.required],
      accountExecutiveId:  [null],
      hrExecutiveId:  [null],
      logisticsExecutiveId:  [null],
      remarks:  [null],
      enquiryItems: this.fb.array([
          this.newEnquiryItem()
        ]),
      });

    this.getProfessions();
    this.getEmployees();
    this.getOfficials();

    this.getClients();

    const enquiryId = +this.activatedRoute.snapshot.paramMap.get('id');

    if (enquiryId) {
        this.pageTitle = 'edit Enquiry';
        this.getEnquiry(enquiryId);
        console.log('after get enquiry in order-edit.component.ts, enquiry items have ');
        console.log(this.enquiry.enquiryItems.length);
     }
  }

  newEnquiryItem(): FormGroup{
    return this.fb.group(
    {
      srNo: [null],
      categoryItemId: [null, Validators.required],
      categoryName:  [null],
      quantity:  [null, [Validators.required, Validators.min(1)]],
      maxCVsToSend:  [null, Validators.required],
      ecnr:  [null, Validators.required],
      assessmentRequired:  [null, Validators.required],
      evaluationRequired:  [null, Validators.required],
      hrExecutiveId:  [null],
      assessingSupId:  [null],
      assessingHRMId:  [null],
      completeBy:  [null],
      reviewStatus:  [null],
      enquiryStatus:  [null],
      charges: ''
    });
  }

  getEnquiry(id: number): any {
    this.enquiry = this.service.getEnquiry(id).subscribe(
      (enq: IEnquiry) => {
        this.editEnquiry(enq);
        this.enquiry = enq;
        console.log('in order-edit.compnent.ts getEquiry, enquiry.EnquiryItems have ');
        console.log(this.enquiry.enquiryItems.length);
      },
      (error: any) => console.log(error)
    );
  }

  editEnquiry(enquiry: IEnquiry): any {
    this.createForm.patchValue(
      {
        id: enquiry.id,
        customerId: enquiry.customerId,
        enquiryRef: enquiry.enquiryRef,
        enquiryNo: enquiry.enquiryNo,
        enquiryDate: new Date(enquiry.enquiryDate).toDateString(),
        basketId: enquiry.basketId,
        completeBy: new Date(enquiry.completeBy).toDateString(),
        reviewStatus: enquiry.reviewStatus,
        readyToReview: enquiry.readyToReview,
        reviewedById:  enquiry.reviewedById,
        reviewedOn:  enquiry.reviewedOn,
        enquiryStatus:  enquiry.enquiryStatus,
        projectManagerId:  enquiry.projectManagerId,
        accountExecutiveId:  enquiry.accountExecutiveId,
        hrExecutiveId:  enquiry.hrExecutiveId,
        logisticsExecutiveId:  enquiry.logisticsExecutiveId,
        remarks:  enquiry.remarks
      });
    console.log('in order-edit.component.ts, enquiryitems have ' );
    console.log( enquiry.enquiryItems.length);

    if (enquiry.enquiryItems !== null || enquiry.enquiryItems.length !== 0) {
      this.createForm.setControl('enquiryItems', this.setExistingEnquiryItems(enquiry.enquiryItems));
      }
  }

  // equiryitems
  setExistingEnquiryItems(items: any): FormArray {
      const formArray = new FormArray([]);
      items.forEach( s => {
          formArray.push(this.fb.group( {
            id: s.id,
            srNo: s.srNo,
            categoryItemId: s.categoryItemId,
            quantity:  s.quantity,
            maxCVsToSend:  s.maxCVsToSend,
            ecnr:  s.ecnr,
            assessmentRequired:  s.assessmentRequired,
            evaluationRequired:  s.evaluationRequired,
            hrExecutiveId:  s.hrExecutiveId,
            assessingSupId:  s.assessingSupId,
            assessingHRMId:  s.assessingHRMId,
            completeBy:  new Date(s.completeBy).toDateString(),
            reviewStatus:  s.reviewStatus,
            enquiryStatus:  s.enquiryStatus,
            charges: s.charges
          }));
        });
      return formArray;
  }

  enquiryItems(): FormArray {
    return this.createForm.get('enquiryItems') as FormArray;
  }

  pushNewEnquiryItem(): void {
    this.enquiryItems().push(this.newEnquiryItem());
  }

  removeEnquiryItem(i: number): any {
    const itemFormArray = this.createForm.get('enquiryItems');
    itemFormArray.markAsDirty();
    itemFormArray.markAsTouched();
    this.enquiryItems().removeAt(i);
  }

  mapFormValuesToEnquiryObject(): IEnquiry{
    this.enquiry.id = this.createForm.value.id ;
    this.enquiry.enquiryNo = this.createForm.value.enquiryNo;
    this.enquiry.customerId = this.createForm.value.customerId ?? 0;
    this.enquiry.enquiryDate = this.createForm.value.enquiryDate;
    this.enquiry.enquiryRef = this.createForm.value.enquiryRef;
    this.enquiry.basketId = this.createForm.value.basketId;
    this.enquiry.completeBy = this.createForm.value.completeBy;
    this.enquiry.reviewStatus = this.createForm.value.reviewStatus;
    this.enquiry.readyToReview = this.createForm.value.readyToReview;
    this.enquiry.reviewedById = this.createForm.value.reviewedById;
    this.enquiry.reviewedOn = this.createForm.value.reviewedOn;
    this.enquiry.enquiryStatus = this.createForm.value.enquiryStatus;
    this.enquiry.projectManagerId = this.createForm.value.projectManagerId;
    this.enquiry.accountExecutiveId = this.createForm.value.accountExecutiveId;
    this.enquiry.hrExecutiveId = this.createForm.value.hrExecutiveId;
    this.enquiry.logisticsExecutiveId = this.createForm.value.logisticsExecutiveId;
    this.enquiry.remarks = this.createForm.value.remarks;
    this.enquiry.enquiryItems = this.createForm.value.enquiryItems;
    return this.enquiry;
  }

  logValidationErrors(group: FormGroup = this.createForm): void {
    Object.keys(group.controls).forEach((key: string) => {
      const abstractControl = group.get(key);

      this.formErrors[key] = '';
      if (abstractControl && !abstractControl.valid &&
        (abstractControl.touched || abstractControl.dirty || abstractControl.value !== ''))
      {
          for (const errorKey in abstractControl.errors) {
            if (errorKey) {
              // this.formErrors[key] += messages[errorKey] + '';
            }
          }
        }

      if (abstractControl instanceof FormGroup) {
        this.logValidationErrors(abstractControl);
      }
    });
  }

  getProfessions(): any {
    this.userService.getProfessions().subscribe(response =>
      { this.professions = response; }
      , error => {
        console.log(error); });
  }

  getEmployees(): any {
    this.empService.getEmployeesData().subscribe( response => {
      this.employees = response;
      // console.log(this.employees);
    }, error => {
      console.log(error);
    });
  }

  getOfficials(): any {
    this.service.getAllOfficials().subscribe( response => {
      this.allOfficials = response;
      console.log('getAllOfficials in order-create-component.ts getOfficials');
      console.log('order-edit component - all officials total records' + this.allOfficials.length);
    }, error => {
      console.log(error);
    });
  }

  getClientOfficials(id: number): any {
    this.service.getCustomerOfficials(id).subscribe( response => {
        this.officials = response;
        console.log('order edit component, getClientOfficials for customer id ' + id + ', total officials: ' + this.officials.length);
      }, error => {
        console.log(error);
      });
    }

  getClients(): any {
    this.service.getClients().subscribe( response => {
      this.customers = response;
    }, error => {

      console.log(error);
    });
  }

  onSubmit(): any {
    const enqVal = this.mapFormValuesToEnquiryObject();
    console.log(enqVal);
    this.service.updateEnquiry(enqVal).subscribe(() => {
    this.route.navigate(['enquiry']);
    }, error => {
      console.log(error);
    });
  }

  onCustomerChanged(id: number): any {
    if (this.clientIdSelected !== id)
    {
      this.getClientOfficials(id);
      this.clientIdSelected = id;
    }
  }
}

