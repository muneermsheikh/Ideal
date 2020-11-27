import { Component, OnInit } from '@angular/core';
import { FormArray, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { EmployeesService } from 'src/app/employees/employees.service';
import { ProfessionService } from 'src/app/profession/profession.service';
import { IClient, IClientOfficial } from 'src/app/shared/models/client';
import { IEmployee } from 'src/app/shared/models/employee';
import { IEnquiryItemToAdd, IEnquiryToAdd } from 'src/app/shared/models/enquiryToAdd';
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
    enquiry: IEnquiryToAdd;
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
                private activatedRoute: ActivatedRoute,
                private route: Router)
    {
    }

    ngOnInit(): void {
      this.createForm = this.fb.group({
        id: [0],
        customerId: [null, Validators.required],
        enquiryRef: [null],
        enquiryDate: [this.todayDate, Validators.required],
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

      this.pageTitle = 'new Enquiry';
      this.blankOutTheFields();
    }

    newEnquiryFormForEdit(): FormGroup {
      return this.fb.group({
      id: [0],
      customerId: [null, Validators.required],
      enquiryRef: [null],
      enquiryNo: [0],
      enquiryDate: [this.todayDate, Validators.required],
      basketId: [null],
      remarks:  [null],
      enquiryItems: this.fb.array([
          this.newEnquiryItem()
        ]),
      });
    }


    newEnquiryItem(): FormGroup{
      return this.fb.group(
      {
        categoryItemId: [null, Validators.required],
        quantity:  [0, [Validators.required, Validators.min(1)]],
        maxCVsToSend:  [0, Validators.required],
        ecnr:  [this.defaultECNR, Validators.required],
        assessmentRequired:  [this.defaultAssessmentReqd, Validators.required],
        evaluationRequired:  [this.defaultCvEvaluationReqd, Validators.required],
        charges: ''
      });
    }

    blankOutTheFields(): void {
      this.enquiry = {
        id: 0,
        customerId: null,
        enquiryRef: '',
        enquiryDate: '',
        remarks:  null,
        enquiryItems: []
      };
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

    mapFormValuesToEnquiryObject(): IEnquiryToAdd{
      this.enquiry.id = this.createForm.value.id ;
      this.enquiry.customerId = this.createForm.value.customerId ?? 0;
      this.enquiry.enquiryDate = this.createForm.value.enquiryDate;
      this.enquiry.enquiryRef = this.createForm.value.enquiryRef;
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
      console.log(enqVal);
      if (enqVal.id === null || enqVal.id === 0)    // INSERT mode
      {
        this.service.addEnquiry(enqVal).subscribe(() => {
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

