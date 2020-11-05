import { Component, OnInit } from '@angular/core';
import { AsyncValidatorFn, FormArray, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { of, timer } from 'rxjs';
import { map, switchMap } from 'rxjs/operators';
import { AccountService } from 'src/app/account/account.service';
import { ProfessionService } from 'src/app/profession/profession.service';
import { IEmployee } from 'src/app/shared/models/employee';
import { IProfession } from 'src/app/shared/models/profession';
import { UsersService } from 'src/app/users/users.service';
import { EmployeesService } from '../employees.service';

@Component({
  selector: 'app-employee-create',
  templateUrl: './employee-create.component.html',
  styleUrls: ['./employee-create.component.scss']
})

export class EmployeeCreateComponent implements OnInit {
  form: FormGroup;
  formArrayAdd: FormArray;
  formArrayCat: FormArray;
  errors: string[];
  validationMessages: string[];

  constructor(private service: EmployeesService,
              private profService: ProfessionService,
              private acctService: AccountService,
              private fb: FormBuilder, private router: Router,
              private activatedRoute: ActivatedRoute,
              private userService: UsersService)
  {
  }

  ngOnInit(): void {
    this.form = this.fb.group ({
      firstName: ['', [Validators.required, Validators.minLength(5)]],
      secondName: [''],
      familyName: [''],
      email: ['', [Validators.required, Validators.email]],
      skills: this.fb.group({
        skillName: [''],
        expInYears: [''],
        proficiency: ['intermediate']
      })
    });
//      email: ['1022@gmail.com', [Validators.required, Validators.email,
//             //Validators.pattern('^[\\w-\\.]+@([\\w-]+\\.)+[\\w-]{2,4}$')]

  }

  onContactPreferenceChange(selectedValue: string): void {
    const phoneControl = this.form.get('mobile');

    if (selectedValue === 'mobile') {

    }
  }

  getEmployee(id: number): void {
    this.service.getEmployeeById(id).subscribe( (emp: IEmployee) => {
      this.editEmployee(emp);
    }, error => {
      console.log(error);
    });
  }

  editEmployee(emp: IEmployee): void {
    this.form.patchValue( {
      id: emp.id,
      firstName: emp.firstName,
      secondName: emp.secondName,
      familyName: emp.familyName,
      gender: emp.gender,
      knownAs: emp.knownAs,
      fullName: emp.fullName,
      dOB: emp.dOB,
      pPNo: emp.pPNo,
      aadharNo: emp.aadharNo,
      mobile: emp.mobile,
      email: emp.email
    });
  }


  createForm(): any {
    return this.form = this.fb.group({
      firstName: [null],
      secondName: [null],
      familyName: [null],
      gender: [null],
      knownAs: [null],
      fullName: [null],
      dOB: [null],
      pPNo: [null],
      aadharNo:  [null, [Validators.minLength(12),
            Validators.pattern('^([0-9]{12})$')]
            ] ,
      mobile: [null, [Validators.required,
            Validators.minLength(10),
            Validators.maxLength(15)]],
      email: ['1022@gmail.com', [Validators.required, Validators.email,
            Validators.pattern('^[\\w-\\.]+@([\\w-]+\\.)+[\\w-]{2,4}$')]
            ],
      addresses: this.fb.array([
        this.addAddressFormGroup()
      ])
      });
  }

  // address formArray
  addAddressFormGroup(): FormGroup {
    return this.fb.group({
      addressType: ['R', [Validators.required]],
      address1: ['25/50 BDD Blocks'],
      address2: ['Worli'],
      city: ['Mumbai', [Validators.required]],
      pin: ['400018'],
      state: ['Maharashtra'],
      district: ['Mumbai'],
      country: ['India', [Validators.required]]
      });
  }

  get AddressControls(): FormArray {
    return this.form.get('formArrayAdd') as FormArray;
  }

  addAddress(): void {
    // this.formArrayAdd = this.form.get('formArrayAdd') as FormArray;
    this.formArrayAdd.push(this.addAddressFormGroup());
  }

  removeAddress(i: number): void {
    this.formArrayAdd.removeAt(i);
  }

  logAddressValue(): void {
    console.log(this.formArrayAdd.value);
  }

// profession formArray

  createProfessionForm(): FormGroup {
    return this.fb.group({
      professionId: [10, [Validators.required]]
    });
  }

  get CategoryControls(): FormArray {
    return this.form.get('formArrayCat') as FormArray;
  }

  addProfession(): void {
    this.formArrayCat = this.form.get('formArrayCat') as FormArray;
    this.formArrayCat.push(this.createProfessionForm());
  }

  removeProfession(i: number): void {
    this.formArrayCat.removeAt(i);
  }

  onSubmit(): any {
    console.log(this.form);
    /*
    this.service.addEmployee(this.form.value).subscribe(response => {

    this.router.navigateByUrl('employees');
    }, error => {
      console.log(error);
      this.errors = error.errors;
    });
    */
  }

  onLoadDataClick(): any {
    this.form.setValue( {
      firstName : 'Vilas',
      secondName : 'Rajaram',
      familyName : 'Kulkarni',
      email : 'vilas@gmail.com',
      skills: {
        skillName: 'browsing portals',
        expInYears : '2',
        proficiency: 'intermediate'
      }
    });
  }

  validateEmailNotTaken(): AsyncValidatorFn {
    return control => {
      return timer(500).pipe(
        switchMap(() => {
          if (!control.value)
          {
            return of(null);
          }
          return this.acctService.checkEmailExists(control.value).pipe(
            map(res => {
              return res ? {emailExists: true} : null;
            })
          );
        })
      );
    };
  }

  validateAadharNumberNotTaken(): AsyncValidatorFn {
    return control => {
      return timer(500).pipe(
        switchMap(() => {
          if (!control.value)
          {
            return of(null);
          }
          return this.userService.checkAadharNoExists(control.value).pipe(
            map(res => {
              return res ? {aadharNoExists: true} : null;
            })
          );
        })
      );
    };
  }

    logValidationErrors(group: FormGroup = this.form): void {

      Object.keys(group.controls).forEach((key: string) => {
        const abstractControl = group.get(key);

        this.form.errors[key] = '';
        if (abstractControl && !abstractControl.valid &&
            (abstractControl.touched || abstractControl.dirty ||
              abstractControl.value !== '')) {
              const messages = this.validationMessages[key];

              for (const errorKey in abstractControl.errors) {
              if (errorKey) {
                this.errors[key] += messages[errorKey] + '';
              }
            }
          }

        if (abstractControl instanceof FormGroup) {
            this.logValidationErrors(abstractControl);
          }
        if (abstractControl instanceof FormArray) {
            for (const control of abstractControl.controls) {
              if ( control instanceof FormGroup) {
                this.logValidationErrors(control);
              }
              }
            }

      });
    }

  }

