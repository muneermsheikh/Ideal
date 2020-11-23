import { Component, OnInit } from '@angular/core';
import { FormArray, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { EmployeesService } from 'src/app/employees/employees.service';
import { ProfessionService } from 'src/app/profession/profession.service';
import { IClient, IClientOfficial } from 'src/app/shared/models/client';
import { IEmployee } from 'src/app/shared/models/employee';
import { IEnquiry, IEnquiryItem } from 'src/app/shared/models/enquiry';
import { IProfession } from 'src/app/shared/models/profession';
import { ClientsService } from 'src/app/users/clients/clients.service';
import { UsersService } from 'src/app/users/users.service';
import { OrdersService } from '../orders.service';

@Component({
  selector: 'app-order-create',
  templateUrl: './order-create.component.html',
  styleUrls: ['./order-create.component.scss']
})
export class OrderCreateComponent implements OnInit {
    createForm: FormGroup;
    enquiry: IEnquiry;
    pageTitle: string;
    professions: IProfession[];
    employees: IEmployee[];
    allOfficials: IClientOfficial[];
    officials: IClientOfficial[];
    customers: IClient[];
    clientIdSelected: number;

    todayDate = new Date();
    defaultDaysCompleteWithin = 7;
    defaultECNR = 'ecr';
    defaultAssessmentReqd = 'f';
    defaultCvEvaluationReqd = 'f';

    formErrors = {
    };

    validationMessages = {
        customerId: {
          required: 'customer is required'
        }
      };


    constructor(private fb: FormBuilder,
                private service: OrdersService,
                private userService: UsersService,
                private empService: EmployeesService,
                private clientService: ClientsService,
                private activatedRoute: ActivatedRoute,
                private route: Router)
    {
    }

    ngOnInit(): void {
      this.createForm = this.fb.group({
        id: [0],
        customerId: [null, Validators.required],
        enquiryRef: [null],
        enquiryNo: [0],
        enquiryDate: [this.todayDate, Validators.required],
        basketId: [null],
        completeBy: [this.todayDate.setDate(this.todayDate.getDate()) + this.defaultDaysCompleteWithin, Validators.required],
        readyToReview: ['f'],
        reviewStatus: ['f'],
        reviewedById:  [0],
        reviewedOn:  [null],
        enquiryStatus:  ['NotStarted', Validators.required],
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
          // edit
          this.pageTitle = 'edit Enquiry';
          this.getEnquiry(enquiryId);
        } else {
          // insert
          this.pageTitle = 'new Enquiry';
          this.blankOutTheFields();
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
        ecnr:  [this.defaultECNR, Validators.required],
        assessmentRequired:  [this.defaultAssessmentReqd, Validators.required],
        evaluationRequired:  [this.defaultCvEvaluationReqd, Validators.required],
        hrExecutiveId:  [null],
        assessingSupId:  [null],
        assessingHRMId:  [null],
        completeBy:  [null],
        reviewStatus:  ['NotReviewed'],
        enquiryStatus:  ['NotStarted'],
        charges: ''
      });
    }

    blankOutTheFields(): void {
      this.enquiry = {
        id: 0,
        customerId: null,
        enquiryRef: '',
        enquiryNo: null,
        enquiryDate: '',
        basketId: null,
        completeBy: null,
        reviewStatus: '',
        readyToReview: 'f',
        reviewedById:  0,
        reviewedOn:  null,
        enquiryStatus:  'NotStarted',
        projectManagerId:  null,
        accountExecutiveId: null,
        hrExecutiveId:  null,
        logisticsExecutiveId: null,
        remarks:  null,
        enquiryItems: []
      };
    }

    getEnquiry(id: number): any {
      this.enquiry = this.service.getEnquiry(id).subscribe(
        (enq: IEnquiry) => {
          this.editEnquiry(enq);
          this.enquiry = enq;
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
          enquiryDate: enquiry.enquiryDate,
          basketId: enquiry.basketId,
          completeBy: enquiry.completeBy,
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

      if (enquiry.enquiryItems !== null || enquiry.enquiryItems.length !== 0) {
        this.createForm.setControl('enquiryItems', this.setExistingEnquiryItems(enquiry.enquiryItems));
        }
    }

    // equiryitems
    setExistingEnquiryItems(items: IEnquiryItem[]): FormArray {
        const formArray = new FormArray([]);
        items.forEach( s => {
          formArray.push(this.fb.group( {
            id: s.id,
            srNo: s.srNo,
            categoryItemId: s.categoryItemId,
            categoryName:  s.categoryName,
            quantity:  s.quantity,
            maxCVsToSend:  s.maxCVsToSend,
            ecnr:  s.ecnr,
            assessmentRequired:  s.assessmentRequired,
            evaluationRequired:  s.evaluationRequired,
            hrExecutiveId:  s.hrExecutiveId,
            assessingSupId:  s.assessingSupId,
            assessingHRMId:  s.assessingHRMId,
            completeBy:  s.completeBy,
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
        console.log(this.allOfficials);
      }, error => {
        console.log(error);
      });
    }

    getClientOfficials(id: number): any {
      this.service.getCustomerOfficials(id).subscribe( response => {
          this.officials = response;
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
      if (enqVal.id === null || enqVal.id === 0)    // INSERT mode
      {
        this.service.addEnquiry(enqVal).subscribe(() => {
          this.route.navigate(['enquiry']);
        }, error => {
          console.log(error);
        });
      } else {                          // EDIT mode
        this.service.updateEnquiry(enqVal).subscribe(() => {
          this.route.navigate(['enquiry']);
      }, error => {
        console.log(error);
      });
      }
    }

    onCustomerChanged(id: number): any {
      console.log('onCustomerChanged id=' + id);
      if (this.clientIdSelected !== id)
      {
        this.getClientOfficials(id);
        this.clientIdSelected = id;
      }
    }
  }

