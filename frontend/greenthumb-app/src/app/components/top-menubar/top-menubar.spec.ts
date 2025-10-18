import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TopMenubar } from './top-menubar';

describe('TopMenubar', () => {
  let component: TopMenubar;
  let fixture: ComponentFixture<TopMenubar>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TopMenubar]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TopMenubar);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
