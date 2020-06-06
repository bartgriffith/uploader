import { Component, Inject } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-create-customer',
  templateUrl: './create-customer.component.html',
  styleUrls: ['./create-customer.component.css']
})
export class CreateCustomerComponent {
  form: FormGroup;
  baseUrl: string;

  constructor(
    public fb: FormBuilder,
    private http: HttpClient,
    @Inject('BASE_URL') baseUrl: string
  ) {
    this.form = this.fb.group({
      name: [''],
      email: [''],
      phone: [''],
      password: ['']
    });

    this.baseUrl = baseUrl;
  }

  submitCustomer() {
    const formData = new FormData();
    formData.append("name", this.form.value.name);
    formData.append("email", this.form.value.email);
    formData.append("phone", this.form.value.phone);
    formData.append("password", this.form.value.password);

    this.http.post(this.baseUrl + 'customer', formData, {responseType: 'text'}).subscribe(
      result => {
        console.log(result);
      },
      err => {
        console.error(err);
      }
    )
  }

}
