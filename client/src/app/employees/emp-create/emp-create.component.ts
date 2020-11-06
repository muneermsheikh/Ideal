import { Component, OnInit } from '@angular/core';
import { AsyncValidatorFn, FormArray, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { of, timer } from 'rxjs';
import { map, switchMap } from 'rxjs/operators';
import { AccountService } from 'src/app/account/account.service';
import { UsersService } from 'src/app/users/users.service';
import { EmployeesService } from '../employees.service';

@Component({
  selector: 'app-emp-create',
  templateUrl: './emp-create.component.html',
  styleUrls: ['./emp-create.component.scss']
})
export class EmpCreateComponent implements OnInit {
  errors: string[];
  form: FormGroup;

  constructor(private fb: FormBuilder, private service: EmployeesService,
              private router: Router, private acctService: AccountService,
              private userService: UsersService) { }

  ngOnInit(): void {
    this.createEmployeeForm();
  }

  createEmployeeForm(): void {
      this.form = this.fb.group ({
        aadharNo: [null, [
          Validators.minLength(12),
          Validators.pattern('^([0-9]{12})$')]],
 //         [this.validateAadharNumberNotTaken]] ,
        passportNo: [null],
        mobile: [null, [
          Validators.required,
          Validators.minLength(10),
          Validators.maxLength(15)]],
        email: [null, [
          Validators.required,
          Validators.pattern('^[\\w-\\.]+@([\\w-]+\\.)+[\\w-]{2,4}$')]],
//          [this.validateEmailNotTaken()]],
        gender: ['M', [
          Validators.required,
          Validators.pattern('^(?:m|M|f|F)$')]],
        firstName:  [null, [Validators.required,
          Validators.maxLength(25), Validators.minLength(5)]],
        secondName: [null],
        familyName: [null],
        knownAs: [null, [Validators.required, Validators.maxLength(15), Validators.minLength(4)]],
        dateOfBirth: [null, [Validators.required]],
        dateOfJoining: [null, [Validators.required]],
        designation: [null, [Validators.required]],
        username: [null],
        password: [null, [Validators.required]],
        addresses: this.fb.array([this.createAddressForm()]),
        employeeSkills: this.fb.array([this.createSkillsForm()])
    });
  }

// address formArray
  createAddressForm(): FormGroup {
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

  get addresses(): FormArray {
    return this.form.get('addresses') as FormArray;
  }
  removeAddress(i: number): void {
    this.addresses.removeAt(i);
  }
  addAddress(): any {
    this.addresses.push(this.createAddressForm());
  }


// skills formArray
  createSkillsForm(): FormGroup {
    return this.fb.group({
      skillName: [null, [Validators.required]],
      expInYears: [null, [Validators.required]],
      proficiency: [null]
    });
  }

  get employeeSkills(): FormArray {
    return this.form.get('employeeSkills') as FormArray;
  }
  removeSkill(i: number): void {
    this.employeeSkills.removeAt(i);
  }
  addSkill(): any {
    this.employeeSkills.push(this.createSkillsForm());
  }

  onSubmit(): void {
    console.log(this.form.value);
    this.service.addEmployee(this.form.value).subscribe(response => {
      this.router.navigateByUrl('/employees');
     }, error => {
      console.log(error);
      this.errors = error.errors;
    });
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

  validateEmailNotTaken(): AsyncValidatorFn {
    return control => {
      return timer(500).pipe(
        switchMap(() => {
          if (!control.value) {
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

}
