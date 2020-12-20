import { Component, OnInit } from '@angular/core';
import { FormArray, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { EmployeesService } from 'src/app/employees/employees.service';
import { IClient, IClientOfficial } from 'src/app/shared/models/client';
import { IEmployee } from 'src/app/shared/models/employee';
import { IEnquiry, IEnquiryItem, IJobDesc, IRemuneration } from 'src/app/shared/models/enquiry';
import { IProfession } from 'src/app/shared/models/profession';
import { ISelStatsDto } from 'src/app/shared/models/selStatsDto';
import { UsersService } from 'src/app/users/users.service';
import { OrdersService } from '../orders.service';

@Component({
  selector: 'app-order-edit',
  templateUrl: './order-edit.component.html',
  styleUrls: ['./order-edit.component.scss']
})
export class OrderEditComponent implements OnInit {
  public isHidden: boolean = true;
  xPosTabMenu: number;
  yPosTabMenu: number;

  // private user: IUser;
  createForm: FormGroup;
  enquiry: IEnquiry;
  pageTitle: string;
  professions: IProfession[];
  employees: IEmployee[];
  allOfficials: IClientOfficial[];
  officials: IClientOfficial[];
  customers: IClient[];
  clientIdSelected: number;
  selStats: ISelStatsDto[];
  
  showRemuneration: boolean;
  showJD: boolean;

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
    this.getClients();
    this.getProfessions();
    this.getEmployees();
    this.getOfficials();
    this.showRemuneration=true;
    this.showJD=true;

    this.createForm = this.fb.group({
      id: [0],
      customerId: [null, Validators.required],
      accountExecutiveId:  [null],
      hrExecutiveId:  [null],
      logisticsExecutiveId:  [null],
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
      remarks:  [null],
      enquiryItems: this.fb.array([
          this.newEnquiryItem()
        ]),
      });

    const enquiryId = +this.activatedRoute.snapshot.paramMap.get('id');

    if (enquiryId) {
        this.pageTitle = 'edit Enquiry';
        this.getEnquiry(enquiryId);
        this.getSelStats(enquiryId);
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
      charges: '',
      jd: this.fb.array([
        this.newJd()
      ]),
      remuneration: this.fb.array([
        this.newRemuneration()
      ])
    });
  }

  newJd(): FormGroup {
    return this.fb.group(
      {
        id: null,
        enquiryItemId: null,
        jobDescription: null,
        qualificationDesired: null,
        experienceDesiredMin: null,
        experienceDesiredMax: null,
        jobProfileDetails: null,
        jobProfileUrl: null
      }
    );
  }

  newRemuneration(): FormGroup {
    return this.fb.group(
      {
        id: null,
        enquiryItemId: null,
        contractPeriodInMonths: null,
        salaryCurrency: null,
        salaryMin: null,
        salaryMax: null,
        salaryNegotiable: null,
        housing: null,
        housingAllowance:  null,
        food: null,
        foodAllowance: null,
        transport: null,
        transportAllowance: null,
        otherAllowance:  null,
        leaveAvailableAfterHowmanyMonths:  null,
        leaveEntitlementPerYear: null,
        updatedOn: null
      }
    );
  }

  getEnquiry(id: number): any {
    this.enquiry = this.service.getEnquiry(id).subscribe(
      (enq: IEnquiry) => {
        this.editEnquiry(enq);
        this.enquiry = enq;
        console.log(enq);
        this.getClientOfficials(enq.customerId);
      },
      (error: any) => console.log(error)
    );
  }

  editEnquiry(enquiry: IEnquiry): any {
      // repopulate customer official dropdowns
      this.getClientOfficials(enquiry.customerId);
      this.createForm.patchValue(
      {
        id: enquiry.id,
        customerId: enquiry.customerId,
        accountExecutiveId:  enquiry.accountExecutiveId,
        hrExecutiveId:  enquiry.hrExecutiveId,
        logisticsExecutiveId:  enquiry.logisticsExecutiveId,
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
        remarks:  enquiry.remarks
      });


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
            enquiryId: s.enquiryId,
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

          console.log(s.jobDesc);
          console.log(s.remuneration);
          if (s.jobDesc !== null) {this.createForm.setControl('jd', this.setExistingJD(s.jobDesc)); }
          if (s.remuneration !== null) {this.createForm.setControl('remuneration', this.setExistingRemuneration(s.remuneration)); }
        });
      return formArray;
  }

  setExistingJD(job: any): FormArray {
    const jdArray = new FormArray([]);
    job.forEach( j => {
      jdArray.push(this.fb.group({
        id: j.id,
        enquiryItemId: j.enquiryItemId,
        jobDescription: j.jobDescription,
        qualificationDesired: j.qualificationDesired,
        experienceDesiredMin: j.experienceDesiredMin,
        experienceDesiredMax: j.experienceDesiredMax,
        jobProfileDetails: j.jobProfileDetails,
        jobProfileUrl: j.jobProfileUrl
      }));
    });
    return jdArray;
  }

  setExistingRemuneration(remun: any): FormArray {
    const rArray = new FormArray([]);
    remun.forEach( r => {
      rArray.push(this.fb.group({
        id: r.id,
        enquiryItemId: r.enquiryItemId,
        contractPeriodInMonths: r.contractPeriodInMonths,
        salaryCurrency: r.salaryCurrency,
        salaryMin: r.salaryMin,
        salaryMax: r.salaryMax,
        salaryNegotiable: r.salaryNegotiable,
        housing: r.housing,
        housingAllowance: r.housingAllowance,
        food: r.food,
        foodAllowance: r.foodAllowance,
        transport: r.transport,
        transportAllowance: r.transportAllowance,
        otherAllowance:  r.otherAllowance,
        leaveAvailableAfterHowmanyMonths: r.leaveAvailableAfterHowmanyMonths,
        leaveEntitlementPerYear: r.leaveEntitlementPerYear,
        updatedOn: r.updatedOn
      }));
    });
    return rArray;
  }

  enquiryItems(): FormArray {
    return this.createForm.get('enquiryItems') as FormArray;
  }

  jd(): FormArray {
    return this.createForm.get('jd') as FormArray;
  }

  remuneration(): FormArray {
    return this.createForm.get('remuneration') as FormArray;
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
      console.log('in onCustomerChanged');
    }
  }

  suggestHRExec(): any {
    // suggest HR Executives based upon past statistics for the relevant category
    
  }

  getSelStats(enquiryId: number)
  {
    this.service.getSelStats(enquiryId).subscribe(response => {
      this.selStats = response;
    }, error => {
      console.log(error);
    }); 
  }

  

  // RIGHT CLICK MENU
  rightClick(event, itemId: number) {
    event.stopPropagation();
    /*
    this.xPosTabMenu = event.clientX;
    this.yPosTabMenu = event.clientY;
    this.isHidden = false;
  */
    
    return false;
  }

  closeRightClickMenu() {
    this.isHidden = true;
  }

  assignHRTasks(){
    
  }

  onClickRemuneration(isChecked: boolean)
  {
    this.showRemuneration = !this.showRemuneration;
  }

  onClickJD(isChecked: boolean)
  {
    this.showJD = !this.showJD;
  }

}

