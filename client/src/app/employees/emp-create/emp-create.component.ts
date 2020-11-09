import { Route } from '@angular/compiler/src/core';
import { Component, OnInit } from '@angular/core';
import { AsyncValidatorFn, FormArray, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { of, timer } from 'rxjs';
import { map, switchMap } from 'rxjs/operators';
import { AccountService } from 'src/app/account/account.service';
import { IEmployee, IEmployeeAddress, ISkill } from 'src/app/shared/models/employee';
import { UsersService } from 'src/app/users/users.service';
import { EmployeesService } from '../employees.service';

@Component({
  selector: 'app-emp-create',
  templateUrl: './emp-create.component.html',
  styleUrls: ['./emp-create.component.scss']
})

export class EmpCreateComponent implements OnInit {

  createForm: FormGroup;
  employee: IEmployee;
  pageTitle: string;

  formErrors = {
   };

  validationMessages = {
      gender: {
        required: 'gender is required'
      },
      firstName: {
        required: 'first name is required',
        minLength: 'first name must be min 4 char long',
        maxLength: 'first Name must be max 25 char long'
      },
      email: {
        required: 'email is required',
        email : 'invalid email'
      },
      mobile: {
        required: 'mobile is required'
      }
    };

  constructor(private fb: FormBuilder,
              private accService: AccountService,
              private usrService: UsersService,
              private empService: EmployeesService,
              private activatedRoute: ActivatedRoute,
              private route: Router) {}

  ngOnInit(): void {
    this.createForm = this.fb.group({
      gender: ['M', [Validators.required,
              Validators.maxLength(1),
              Validators.pattern('^(?:m|M|f|F)$')]],
      firstName: ['Sumit', [Validators.required, Validators.maxLength(25), Validators.minLength(4)]],
      secondName: 'Rajaram',
      familyName: 'Thakkar',
      knownAs: ['Sumit', [Validators.required, Validators.minLength(4), Validators.maxLength(15)]],
      aadharNo: ['123456789012', [Validators.minLength(12), Validators.maxLength(12)]],
               // [this.validateAadharNumberNotTaken]],
      passportNo: 'L8349885',
      mobile: ['1234567890', [Validators.required]],
      email: ['sumit@gmail.com', [Validators.required]],
      dateOfBirth: ['1995-10-10T00:00:00', [Validators.required]],
      dateOfJoining: ['2020-01-01T00:00:00', [Validators.required]],
      designation: ['HR Officer', [Validators.required]],
      department: ['Admin', [Validators.required]],
      skills: this.fb.array([
        this.newSkill()
      ]),
      addresses: this.fb.array([
        this.newAddress()
      ]),
    });

  /*
    this.createForm.get('aadharNo').valueChanges.subscribe((data) => {
      const aadharNoTaken = this.validateAadharNumberNotTaken;
      if (aadharNoTaken) {
      }
    });
  */

    /*
    this.activatedRoute.paramMap.subscribe(params => {
      const empId = +params.get('id');
      console.log(empId);
      if (empId) {
        console.log(empId);
        this.getEmployee(empId);
      }
    });
    */

    const empId = +this.activatedRoute.snapshot.paramMap.get('id');

    if (empId) {
      // edit
      this.pageTitle = 'edit employee';
      this.getEmployee(empId);
    } else {
      // insert
      this.pageTitle = 'create a new employee';
      this.blankOutTheFields();
      }
  }

    blankOutTheFields(): void {
        this.employee = {
          id: null,
          gender: 'M',
          firstName: '',
          secondName: '',
          familyName: '',
          knownAs: '',
          aadharNo: '',
          passportNo: '',
          mobile: '',
          email: '',
          dateOfBirth: null,
          dateOfJoining: null,
          designation: '',
          department: '',
          fullName: '',
          skills: [],
          addresses: []
      };
    }

    getEmployee(id: number): any {
      this.employee = this.empService.getEmployeeById(id).subscribe(
        (employee: IEmployee) => {
          this.editEmployee(employee);
          this.employee = employee;
          console.log(employee);
        },
        (error: any) => console.log(error)
      );
    }

    editEmployee(employee: IEmployee): any {
    this.createForm.patchValue({
        gender: employee.gender,
        firstName: employee.firstName,
        secondName: employee.secondName,
        familyName: employee.familyName,
        knownAs: employee.knownAs,
        aadharNo: employee.aadharNo,
        passportNo: employee.passportNo,
        mobile: employee.mobile,
        email: employee.email,
        dateOfBirth: employee.dateOfBirth,
        dateOfJoining: employee.dateOfJoining,
        designation: employee.designation,
        department: employee.department
      });

    if (!employee.skills === null) {
      console.log(employee.skills.length);
      this.createForm.setControl('skills', this.setExistingSkills(employee.skills));
      }

    if (!employee.addresses === null) {
      this.createForm.setControl('addresses', this.setExistingAddresses(employee.addresses));
    }

    }

 // skills
    setExistingSkills(skillSets: ISkill[]): FormArray {
      const formArray = new FormArray([]);
      skillSets.forEach( s => {
        formArray.push(this.fb.group( {
          skillName: s.skillName,
          expInYears: s.expInYears,
          proficiency: s.proficiency
        }));
      });

      return formArray;
    }

    skills(): FormArray {
      return this.createForm.get('skills') as FormArray;
    }


    newSkill(): FormGroup{
      return this.fb.group({
        skillName: '',
        expInYears: '',
        proficiency: '',
      });
    }

    addSkill(): void {
      this.skills().push(this.newSkill());
    }

    removeSkill(i: number): void {

      const skillsFormArray = this.createForm.get('skills');
      skillsFormArray.markAsDirty();
      skillsFormArray.markAsTouched();
      this.skills().removeAt(i);
    }

// addresses

    setExistingAddresses(adds: IEmployeeAddress[]): FormArray {
    const formArray = new FormArray([]);
    adds.forEach( s => {
      formArray.push(this.fb.group( {
        addressType: s.addressType,
        address1: s.address1,
        address2: s.address2,
        city: s.city,
        pin: s.pin,
        state: s.state,
        country: s.country
      }));
    });

    return formArray;
    }

    addresses(): FormArray {
      return this.createForm.get('addresses') as FormArray;
    }

    newAddress(): FormGroup{
      return this.fb.group({
        addressType: ['R', [Validators.required]],
        address1: '12/56 BDD Chawls',
        address2: 'worli',
        city: ['Mumbai', [Validators.required]],
        pin: '400018',
        state: 'Mah',
        country: ['India', [Validators.required]],
      });
    }

    addAddress(): void {
      this.addresses().push(this.newAddress());
    }

    removeAddress(i: number): void {
      const addsFormArray = this.createForm.get('addresses');
      addsFormArray.markAsDirty();
      addsFormArray.markAsTouched();
      this.addresses().removeAt(i);

    }

    onSubmit(): any {
      // console.log(this.createForm.value);
      const empVal = this.mapFormValuesToEmployeeObject();
      console.log(empVal);
      if (this.employee.id !== null)    // INSERT mode
      {
      this.empService.addEmployee(empVal).subscribe(() => {
          this.route.navigate(['employees']);
        }, error => {
          console.log(error);
        });
      } else {                          // EDIT mode
        this.empService.updateEmployee(empVal).subscribe(() => {
          this.route.navigate(['employees']);
      }, error => {
        console.log(error);
      });
      }

    }

    mapFormValuesToEmployeeObject(): IEmployee{
        this.employee.id = this.createForm.value.id;
        this.employee.gender = this.createForm.value.gender;
        this.employee.firstName = this.createForm.value.firstName;
        this.employee.secondName = this.createForm.value.secondName;
        this.employee.familyName = this.createForm.value.familyName;
        this.employee.knownAs = this.createForm.value.knownAs;
        this.employee.aadharNo = this.createForm.value.aadharNo;
        this.employee.email = this.createForm.value.email;
        this.employee.mobile = this.createForm.value.mobile;
        this.employee.dateOfBirth = this.createForm.value.dateOfBirth;
        this.employee.dateOfJoining = this.createForm.value.dateOfJoining;
        this.employee.designation = this.createForm.value.designation;
        this.employee.department = this.createForm.value.department;
        this.employee.skills = this.createForm.value.skills;
        this.employee.addresses = this.createForm.value.addresses;
        return this.employee;
    }

    logValidationErrors(group: FormGroup = this.createForm): void {
      Object.keys(group.controls).forEach((key: string) => {
        const abstractControl = group.get(key);

        this.formErrors[key] = '';
        if (abstractControl && !abstractControl.valid &&
          (abstractControl.touched || abstractControl.dirty || abstractControl.value !== '')) {

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

    validateEmailNotTaken(): AsyncValidatorFn {
      return control => {
        return timer(500).pipe(
          switchMap(() => {
            if (!control.value)
            {
              return of(null);
            }
            return this.accService.checkEmailExists(control.value).pipe(
              map(res => {
                return res ? {emailExists: true} : null;
              })
            );
          })
        );
      };
    }

    validatePPNumberNotTaken(): AsyncValidatorFn {
      return control => {
        return timer(500).pipe(
          switchMap(() => {
            if (!control.value)
            {
              return of(null);
            }
            return this.usrService.checkPPNoExists(control.value).pipe(
              map(res => {
                return res ? {ppNoExists: true} : null;
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
            return this.usrService.checkAadharNoExists(control.value).pipe(
              map(res => {
                return res ? {aadharNoExists: true} : null;
              })
            );
          })
        );
      };
    }


}
