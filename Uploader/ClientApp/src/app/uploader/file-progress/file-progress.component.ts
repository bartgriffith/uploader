import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-file-progress',
  templateUrl: './file-progress.component.html',
  styleUrls: ['./file-progress.component.css']
})
export class FileProgressComponent {
  @Input() progress: number;
  @Input() filename: string;
}
