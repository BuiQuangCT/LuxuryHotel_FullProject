# LUXURY HOTEL - FRONTEND UI/UX DESIGN SPECIFICATIONS

---

## I. DESIGN SYSTEM

### 1.1 Color Palette

#### Primary Colors

```
Primary Blue (Dark):     #2c3e50
  - RGB: (44, 62, 80)
  - Usage: Header, Footer, Sidebar, Primary Text
  - Hex Code: #2c3e50

Primary Gold (Accent):   #d4af37
  - RGB: (212, 175, 55)
  - Usage: Links, Buttons, Hover States, Logo, Highlights
  - Hex Code: #d4af37
  - Lighter Hover: #b5952f
  - RGB: (181, 149, 47)

```

#### Secondary Colors

```
Light Gray (Background):  #f9f9f9 / #f4f7f6
  - RGB: (249, 249, 249) / (244, 247, 246)
  - Usage: Page backgrounds, Card backgrounds

White:                    #ffffff
  - RGB: (255, 255, 255)
  - Usage: Content areas, Cards

Dark Gray (Text):         #333333 / #2c3e50
  - RGB: (51, 51, 51) / (44, 62, 80)
  - Usage: Body text, Labels

Medium Gray:              #7f8c8d / #95a5a6
  - RGB: (127, 140, 141) / (149, 165, 166)
  - Usage: Secondary text, borders, disabled states

Error Red:                #e74c3c
  - RGB: (231, 76, 60)
  - Usage: Error messages, delete buttons, warnings

Success Green:            #27ae60
  - RGB: (39, 174, 96)
  - Usage: Success messages, confirmation buttons

Warning Yellow:           #f39c12
  - RGB: (243, 156, 18)
  - Usage: Warning messages, pending status

Light Warning:            #fff9e6
  - RGB: (255, 249, 230)
  - Usage: Warning background, payment summary background

```

### 1.2 Typography

#### Font Family

```
Primary Font: "Segoe UI", Tahoma, Geneva, Verdana, sans-serif
Fallback Chain:
  1. "Segoe UI" (Windows)
  2. Tahoma (All systems)
  3. Geneva (macOS)
  4. Verdana (All systems)
  5. sans-serif (Generic fallback)

Font: Segoe UI is chosen for its clean, professional appearance
Best for: Web interfaces, readable at small sizes
```

#### Font Sizes Scale

```
H1 (Extra Large):     28-32px  (font-weight: bold)
H2 (Large Heading):   24px     (font-weight: bold)
H3 (Heading):         22px     (font-weight: bold)
H4 (Subheading):      20px     (font-weight: 600)
Body Large:           16px     (font-weight: 400)
Body Regular:         14px     (font-weight: 400)
Body Small:           13px     (font-weight: 400)
Caption:              12px     (font-weight: 400)
Button Text:          14-16px  (font-weight: bold)
Label:                13-14px  (font-weight: 500)
```

#### Font Weights

```
Regular:    400  (Body text, standard weight)
Medium:     500  (Labels, secondary text)
Semibold:   600  (Subheadings, navigation items)
Bold:       700  (Headings, important text)
```

#### Line Heights

```
Heading:      1.2-1.3
Body:         1.5-1.6
Compact:      1.4
```

#### Letter Spacing

```
Logo:         2px    (Letter-spacing for luxury feel)
Heading:      0-1px
Body:         0px (default)
```

### 1.3 Spacing System

#### Base Unit: 8px

```
XS:  4px   (0.5 unit)
S:   8px   (1 unit)    - Default spacing
M:   16px  (2 units)   - Medium spacing
L:   24px  (3 units)   - Large spacing
XL:  32px  (4 units)   - Extra large
2XL: 48px  (6 units)   - Double extra large
3XL: 64px  (8 units)   - Triple extra large
```

#### Common Spacing Values

```
Padding:          10-20px (Cards)
              15-30px (Containers)
              50px (Large sections)

Margin:           10-15px (Form elements)
              20-30px (Sections)

Gap (Flexbox):    15-20px (Horizontal spacing)
              10-20px (Vertical spacing)

Border Radius:    5px (Buttons, inputs)
              10px (Cards)
              15px (Large containers)
```

### 1.4 Shadow System

#### Box Shadows

```
Elevation 1 (Cards):
  0 4px 6px rgba(0, 0, 0, 0.1)

Elevation 2 (Hover):
  0 8px 12px rgba(0, 0, 0, 0.15)

Elevation 3 (Dropdowns):
  0 12px 24px rgba(0, 0, 0, 0.2)

Elevation 4 (Modals):
  0 20px 25px rgba(0, 0, 0, 0.25)

Header Shadow:
  0 4px 6px rgba(0, 0, 0, 0.1)

Login Form Shadow (Dark):
  0 15px 25px rgba(0, 0, 0, 0.5)
```

### 1.5 Animations & Transitions

#### Timing Functions

```
Standard:     0.3s ease        (Button hover, color changes)
Transition:   0.3s ease-in-out (Opacity, transforms)
Fast:         0.1s ease        (Quick feedback)
Slow:         0.5s ease        (Large transitions)
```

#### Common Animations

```
Fade In:
  @keyframes fadeIn {
    from { opacity: 0; }
    to { opacity: 1; }
  }
  Duration: 0.3s ease-in-out

Slide In:
  @keyframes slideIn {
    from { transform: translateY(10px); opacity: 0; }
    to { transform: translateY(0); opacity: 1; }
  }
  Duration: 0.3s ease-out

Spin (Loading):
  @keyframes spin {
    0% { transform: rotate(0deg); }
    100% { transform: rotate(360deg); }
  }
  Duration: 1s linear infinite

Hover:
  Duration: 0.3s
  Properties: background-color, color, box-shadow
```

---

## II. COMPONENT LIBRARY

### 2.1 Buttons

#### Primary Button (CTA - Call to Action)

```css
.btn-primary {
  background-color: #d4af37; /* Gold */
  color: white;
  border: none;
  padding: 12px 20px;
  border-radius: 5px;
  font-size: 16px;
  font-weight: bold;
  cursor: pointer;
  transition: background-color 0.3s;
}

.btn-primary:hover {
  background-color: #b5952f; /* Darker gold */
}

.btn-primary:active {
  transform: scale(0.98);
}
```

#### Secondary Button

```css
.btn-secondary {
  background-color: #95a5a6; /* Gray */
  color: white;
  border: none;
  padding: 12px 20px;
  border-radius: 5px;
  font-weight: bold;
  cursor: pointer;
  transition: background-color 0.3s;
}

.btn-secondary:hover {
  background-color: #7f8c8d; /* Darker gray */
}
```

#### Danger Button (Delete/Cancel)

```css
.btn-danger {
  background-color: #e74c3c; /* Red */
  color: white;
  border: none;
  padding: 12px 20px;
  border-radius: 5px;
  font-weight: bold;
  cursor: pointer;
  transition: background-color 0.3s;
}

.btn-danger:hover {
  background-color: #c0392b; /* Darker red */
}
```

#### Outline Button (Secondary action)

```css
.btn-outline {
  background-color: transparent;
  color: #d4af37; /* Gold text */
  border: 1px solid #d4af37;
  padding: 12px 20px;
  border-radius: 5px;
  font-weight: bold;
  cursor: pointer;
  transition: all 0.3s;
}

.btn-outline:hover {
  background-color: #d4af37;
  color: white;
}
```

#### Logout Button (Outline with red)

```css
.btn-logout {
  background: transparent;
  color: #e74c3c;
  border: 1px solid #e74c3c;
  padding: 5px 15px;
  border-radius: 5px;
  cursor: pointer;
  transition: all 0.3s;
}

.btn-logout:hover {
  background: #e74c3c;
  color: white;
}
```

### 2.2 Form Elements

#### Input Fields

```css
.input-field {
  width: 100%;
  padding: 10px;
  border: none;
  border-radius: 5px;
  background-color: rgba(255, 255, 255, 0.2); /* For login/register */
  color: white;
  outline: none;
  font-family: inherit;
  font-size: 14px;
  transition: background-color 0.3s;
}

.input-field::placeholder {
  color: #ddd;
}

.input-field:focus {
  background-color: rgba(255, 255, 255, 0.3);
  box-shadow: 0 0 0 2px rgba(212, 175, 55, 0.3);
}

/* Standard input (non-transparent) */
.input-standard {
  width: 100%;
  padding: 10px;
  border: 1px solid #ddd;
  border-radius: 5px;
  background-color: white;
  color: #333;
  font-size: 14px;
}

.input-standard:focus {
  outline: none;
  border-color: #d4af37;
  box-shadow: 0 0 0 2px rgba(212, 175, 55, 0.2);
}
```

#### Labels

```css
.form-label {
  display: block;
  margin-bottom: 5px;
  font-size: 14px;
  font-weight: 500;
  color: #2c3e50;
}

.form-label.required::after {
  content: " *";
  color: #e74c3c;
}
```

#### Form Group

```css
.form-group {
  margin-bottom: 15px;
}

.form-group:last-child {
  margin-bottom: 0;
}
```

#### Select Dropdown

```css
.form-select {
  width: 100%;
  padding: 10px;
  border: 1px solid #ddd;
  border-radius: 5px;
  background-color: white;
  color: #333;
  font-size: 14px;
  cursor: pointer;
}

.form-select:focus {
  outline: none;
  border-color: #d4af37;
}
```

#### Checkbox & Radio

```css
.form-checkbox,
.form-radio {
  margin-right: 8px;
  cursor: pointer;
  accent-color: #d4af37;
}

.form-checkbox:checked,
.form-radio:checked {
  accent-color: #d4af37;
}
```

### 2.3 Cards

#### Standard Card

```css
.card {
  background: white;
  padding: 30px;
  border-radius: 10px;
  box-shadow: 0 4px 15px rgba(0, 0, 0, 0.1);
  margin-bottom: 20px;
}

.card.elevated {
  box-shadow: 0 8px 25px rgba(0, 0, 0, 0.15);
}
```

#### Card Header

```css
.card-title {
  font-size: 22px;
  color: #2c3e50;
  margin-bottom: 20px;
  border-bottom: 2px solid #d4af37;
  padding-bottom: 10px;
  display: inline-block;
}
```

#### Card Body

```css
.card-body {
  padding: 20px 0;
}

.card-body p {
  margin: 10px 0;
  line-height: 1.6;
}
```

#### Card Footer

```css
.card-footer {
  border-top: 1px solid #eee;
  padding-top: 20px;
  margin-top: 20px;
}
```

### 2.4 Navigation & Headers

#### Header Layout

```css
header {
  background-color: #2c3e50;
  color: white;
  padding: 15px 50px;
  display: flex;
  justify-content: space-between;
  align-items: center;
  position: sticky;
  top: 0;
  z-index: 1000;
  box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);
}
```

#### Logo

```css
.logo {
  font-size: 24px;
  font-weight: bold;
  color: #d4af37;
  letter-spacing: 2px;
  cursor: pointer;
}

.logo:hover {
  opacity: 0.8;
}
```

#### Navigation Links

```css
.nav-links {
  list-style: none;
  display: flex;
  gap: 20px;
}

.nav-links li {
  cursor: pointer;
  font-weight: 500;
  transition: color 0.3s;
}

.nav-links li:hover {
  color: #d4af37;
}

.nav-links li.active {
  color: #d4af37;
  border-bottom: 2px solid #d4af37;
}
```

#### Sidebar (Admin)

```css
.sidebar {
  width: 250px;
  background-color: #2c3e50;
  color: white;
  padding: 20px;
  display: flex;
  flex-direction: column;
  position: sticky;
  top: 0;
  height: 100vh;
  overflow-y: auto;
}

.sidebar h2 {
  text-align: center;
  color: #d4af37;
  margin-bottom: 30px;
  font-size: 20px;
}

.sidebar ul {
  list-style: none;
  flex: 1;
}

.sidebar ul li {
  padding: 15px;
  border-bottom: 1px solid #34495e;
  cursor: pointer;
  transition: background-color 0.3s;
  font-weight: 500;
}

.sidebar ul li:hover,
.sidebar ul li.active {
  background-color: #d4af37;
  color: white;
}
```

### 2.5 Tables

#### Table Container

```css
.table-container {
  overflow-x: auto;
  border-radius: 10px;
  box-shadow: 0 4px 15px rgba(0, 0, 0, 0.1);
}

table {
  width: 100%;
  border-collapse: collapse;
  font-size: 14px;
}

table thead {
  background-color: #2c3e50;
  color: white;
  font-weight: bold;
}

table th {
  padding: 15px;
  text-align: left;
  border-right: 1px solid #ddd;
}

table th:last-child {
  border-right: none;
}

table tbody tr {
  border-bottom: 1px solid #eee;
  transition: background-color 0.2s;
}

table tbody tr:hover {
  background-color: #f9f9f9;
}

table td {
  padding: 15px;
  border-right: 1px solid #eee;
}

table td:last-child {
  border-right: none;
}

table tbody tr:last-child {
  border-bottom: none;
}
```

### 2.6 Modals & Overlays

#### Loading Spinner

```css
.loading-overlay {
  display: none;
  position: fixed;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  background: rgba(255, 255, 255, 0.8);
  z-index: 1000;
  justify-content: center;
  align-items: center;
  flex-direction: column;
}

.loading-overlay.active {
  display: flex;
}

.spinner {
  border: 4px solid #f3f3f3;
  border-top: 4px solid #d4af37;
  border-radius: 50%;
  width: 40px;
  height: 40px;
  animation: spin 1s linear infinite;
  margin-bottom: 10px;
}

@keyframes spin {
  0% {
    transform: rotate(0deg);
  }
  100% {
    transform: rotate(360deg);
  }
}

.spinner-text {
  color: #2c3e50;
  font-weight: bold;
}
```

#### Modal Dialog

```css
.modal {
  display: none;
  position: fixed;
  z-index: 2000;
  left: 0;
  top: 0;
  width: 100%;
  height: 100%;
  background-color: rgba(0, 0, 0, 0.5);
  justify-content: center;
  align-items: center;
}

.modal.active {
  display: flex;
}

.modal-content {
  background-color: white;
  padding: 30px;
  border-radius: 10px;
  max-width: 500px;
  width: 90%;
  box-shadow: 0 20px 25px rgba(0, 0, 0, 0.25);
}

.modal-header {
  font-size: 20px;
  font-weight: bold;
  margin-bottom: 20px;
  color: #2c3e50;
}

.modal-body {
  margin-bottom: 20px;
  color: #333;
  line-height: 1.6;
}

.modal-footer {
  display: flex;
  gap: 10px;
  justify-content: flex-end;
}

.modal-close {
  background: none;
  border: none;
  font-size: 24px;
  color: #999;
  cursor: pointer;
}
```

### 2.7 Messages & Alerts

#### Success Message

```css
.message-success {
  background-color: #d4edda;
  border: 1px solid #c3e6cb;
  color: #155724;
  padding: 15px;
  border-radius: 5px;
  margin-bottom: 20px;
}
```

#### Error Message

```css
.message-error {
  background-color: #f8d7da;
  border: 1px solid #f5c6cb;
  color: #721c24;
  padding: 15px;
  border-radius: 5px;
  margin-bottom: 20px;
}
```

#### Warning Message

```css
.message-warning {
  background-color: #fff3cd;
  border: 1px solid #ffeeba;
  color: #856404;
  padding: 15px;
  border-radius: 5px;
  margin-bottom: 20px;
}
```

#### Info Message

```css
.message-info {
  color: #0066cc;
  font-size: 14px;
  margin: 10px 0;
}
```

### 2.8 Status Badges

#### Booking Status

```css
.status-badge {
  display: inline-block;
  padding: 5px 12px;
  border-radius: 20px;
  font-size: 12px;
  font-weight: bold;
  text-transform: uppercase;
}

.status-pending {
  background-color: #fff3cd;
  color: #856404;
}

.status-success {
  background-color: #d4edda;
  color: #155724;
}

.status-cancelled {
  background-color: #f8d7da;
  color: #721c24;
}

.status-active {
  background-color: #d1ecf1;
  color: #0c5460;
}
```

---

## III. LAYOUT PATTERNS

### 3.1 Hero Section Pattern

```css
.hero {
  height: 60vh;
  background:
    linear-gradient(rgba(0, 0, 0, 0.4), rgba(0, 0, 0, 0.4)),
    url("image-path") center/cover;
  display: flex;
  flex-direction: column;
  justify-content: center;
  align-items: center;
  text-align: center;
  color: white;
  position: relative;
}

.hero-content {
  z-index: 1;
}

.hero h1 {
  font-size: 48px;
  font-weight: bold;
  margin-bottom: 20px;
  text-shadow: 2px 2px 4px rgba(0, 0, 0, 0.5);
}

.hero p {
  font-size: 20px;
  margin-bottom: 30px;
  text-shadow: 1px 1px 2px rgba(0, 0, 0, 0.5);
}
```

### 3.2 Container Pattern

```css
.container {
  max-width: 1200px;
  margin: 0 auto;
  padding: 0 20px;
}

.container-sm {
  max-width: 600px;
  margin: 0 auto;
}

.container-md {
  max-width: 800px;
  margin: 0 auto;
}

.container-lg {
  max-width: 1400px;
  margin: 0 auto;
}
```

### 3.3 Grid Pattern

```css
.grid {
  display: grid;
  gap: 20px;
}

.grid-2 {
  grid-template-columns: repeat(2, 1fr);
}

.grid-3 {
  grid-template-columns: repeat(3, 1fr);
}

.grid-4 {
  grid-template-columns: repeat(4, 1fr);
}

@media (max-width: 768px) {
  .grid-2,
  .grid-3,
  .grid-4 {
    grid-template-columns: 1fr;
  }
}
```

### 3.4 Flexbox Pattern

```css
.flex {
  display: flex;
  gap: 15px;
}

.flex-between {
  display: flex;
  justify-content: space-between;
  align-items: center;
}

.flex-center {
  display: flex;
  justify-content: center;
  align-items: center;
}

.flex-column {
  display: flex;
  flex-direction: column;
}

.flex-wrap {
  flex-wrap: wrap;
}

.flex-1 {
  flex: 1;
}
```

---

## IV. RESPONSIVE DESIGN

### 4.1 Breakpoints

```css
/* Mobile First Approach */

/* Extra Small (Mobile) */
@media (max-width: 480px) {
  /* CSS for mobile devices */
}

/* Small (Tablet Portrait) */
@media (min-width: 481px) and (max-width: 768px) {
  /* CSS for tablets */
}

/* Medium (Tablet Landscape) */
@media (min-width: 769px) and (max-width: 1024px) {
  /* CSS for larger tablets */
}

/* Large (Desktop) */
@media (min-width: 1025px) {
  /* CSS for desktops */
}

/* Extra Large (Large Desktop) */
@media (min-width: 1441px) {
  /* CSS for very large screens */
}
```

### 4.2 Responsive Typography

```css
/* Mobile */
h1 {
  font-size: 28px;
}
h2 {
  font-size: 22px;
}
body {
  font-size: 14px;
}

/* Tablet */
@media (min-width: 768px) {
  h1 {
    font-size: 36px;
  }
  h2 {
    font-size: 26px;
  }
  body {
    font-size: 15px;
  }
}

/* Desktop */
@media (min-width: 1024px) {
  h1 {
    font-size: 48px;
  }
  h2 {
    font-size: 32px;
  }
  body {
    font-size: 16px;
  }
}
```

### 4.3 Responsive Navigation

```css
/* Mobile Navigation */
.nav-links {
  display: none;
  flex-direction: column;
  gap: 10px;
  position: absolute;
  top: 60px;
  right: 0;
  background: #2c3e50;
  padding: 20px;
  width: 200px;
}

.nav-links.active {
  display: flex;
}

.hamburger {
  display: block;
  font-size: 24px;
  cursor: pointer;
}

/* Desktop Navigation */
@media (min-width: 768px) {
  .nav-links {
    display: flex;
    position: static;
    background: none;
    padding: 0;
    width: auto;
    flex-direction: row;
  }

  .hamburger {
    display: none;
  }
}
```

---

## V. ACCESSIBILITY

### 5.1 WCAG Compliance

```css
/* Focus Styles - Keyboard Navigation */
a:focus,
button:focus,
input:focus,
select:focus,
textarea:focus {
  outline: 2px solid #d4af37;
  outline-offset: 2px;
}

/* High Contrast */
.high-contrast {
  background-color: #000;
  color: #fff;
}

/* Larger Touch Targets */
button,
a {
  min-height: 44px;
  min-width: 44px;
  padding: 8px 12px;
}

/* Color Not Only Indicator */
.form-error {
  border: 2px solid #e74c3c;
  color: #e74c3c;
}

.form-error::after {
  content: " ✖";
}
```

### 5.2 ARIA Labels

```html
<!-- ARIA for screen readers -->
<button aria-label="Close menu">×</button>
<div role="alert">Error message</div>
<img alt="Hotel lobby" src="image.jpg" />
<label for="email">Email Address:</label>
<input id="email" type="email" />
```

---

## VI. GLASSMORPHISM DESIGN (Login/Register)

### 6.1 Frosted Glass Effect

```css
.login-container {
  background: rgba(255, 255, 255, 0.1); /* Transparency */
  backdrop-filter: blur(10px); /* Blur effect */
  border: 1px solid rgba(255, 255, 255, 0.2); /* Subtle border */
  padding: 40px;
  border-radius: 15px;
  width: 400px;
  box-shadow: 0 15px 25px rgba(0, 0, 0, 0.5);
  color: white;
}

.login-container::before {
  content: "";
  position: absolute;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  background: rgba(255, 255, 255, 0.1);
  border-radius: 15px;
  pointer-events: none;
}
```

---

## VII. PAGE-SPECIFIC DESIGN SPECIFICATIONS

### 7.1 Homepage (index.html)

```
Layout: Hero + Content Sections + Footer

Hero Section:
  - Height: 60vh
  - Background: Luxury hotel image with overlay
  - Content: Title + CTA button
  - Color: White text on dark overlay

Featured Rooms Section:
  - Layout: Grid (3 columns on desktop, 1 on mobile)
  - Card height: 400px
  - Image height: 250px
  - Content: Room name, price, amenities

Testimonials Section:
  - Layout: Carousel or grid
  - Avatar: 60px circle
  - Rating: 5-star visualization
  - Text: Italicized review

Footer:
  - Background: Dark (#2c3e50)
  - Layout: 3-4 columns
  - Color: White text
```

### 7.2 Login Page (login.html)

```
Layout: Full screen with background

Background:
  - Hotel image with dark overlay
  - Opacity: 60%
  - Blur: None

Form Container:
  - Width: 400px (responsive)
  - Background: Glassmorphism effect
  - Position: Center screen
  - Elevation: High shadow

Form Elements:
  - Input: Transparent background
  - Focus: Brighter transparency
  - Placeholder: Light gray (#ddd)

Button:
  - Width: 100%
  - Height: 48px
  - Text: Bold white
```

### 7.3 Admin Dashboard (admin.html)

```
Layout: Sidebar + Main Content

Sidebar:
  - Width: 250px
  - Fixed: sticky
  - Overflow: auto
  - Navigation: List items

Main Content:
  - Flex: 1
  - Overflow: auto
  - Padding: 30px

Tab Navigation:
  - Sticky: Top
  - Buttons: Flex layout
  - Active: Gold underline

Content Areas:
  - Cards: Statistics display
  - Tables: Data management
  - Charts: Analytics visualization
```

---

## VIII. MICRO-INTERACTIONS

### 8.1 Hover States

```css
/* Button Hover */
button {
  transition: background-color 0.3s ease;
}

button:hover {
  background-color: darker_shade;
}

/* Link Hover */
a {
  transition: color 0.3s ease;
}

a:hover {
  color: #d4af37;
}

/* Card Hover */
.card {
  transition: box-shadow 0.3s ease;
}

.card:hover {
  box-shadow: 0 8px 20px rgba(0, 0, 0, 0.15);
  transform: translateY(-2px);
}
```

### 8.2 Focus States

```css
/* Form Focus */
input:focus {
  box-shadow: 0 0 0 3px rgba(212, 175, 55, 0.3);
  border-color: #d4af37;
}

/* Button Focus */
button:focus {
  outline: 2px solid #d4af37;
  outline-offset: 2px;
}
```

### 8.3 Loading States

```css
/* Button Loading */
button:disabled {
  opacity: 0.6;
  cursor: not-allowed;
}

/* Input Disabled */
input:disabled {
  background-color: #f0f0f0;
  cursor: not-allowed;
}

/* Loading Animation */
.spinner {
  animation: spin 1s linear infinite;
}
```

---

## IX. PERFORMANCE OPTIMIZATION

### 9.1 CSS Best Practices

```css
/* Minimize repaints */
.transition-optimize {
  will-change: transform, opacity;
}

/* Use GPU acceleration */
.gpu-accelerated {
  transform: translateZ(0);
  backface-visibility: hidden;
  perspective: 1000px;
}

/* Debounce animations */
@media (prefers-reduced-motion: reduce) {
  * {
    animation-duration: 0.01ms !important;
    animation-iteration-count: 1 !important;
    transition-duration: 0.01ms !important;
  }
}
```

### 9.2 Image Optimization

```html
<!-- Responsive Images -->
<img
  src="image.jpg"
  alt="Description"
  loading="lazy"
  srcset="image-sm.jpg 480w, image-md.jpg 768w, image-lg.jpg 1200w"
/>

<!-- WebP Format -->
<picture>
  <source srcset="image.webp" type="image/webp" />
  <img src="image.jpg" alt="Description" />
</picture>
```

---

**Design System Document**  
**Luxury Hotel Management System**  
**Version:** 1.0  
**Last Updated:** 2026-05-14

---

## APPENDIX: CSS UTILITIES

### Quick Reference Classes

```css
/* Spacing Utilities */
.mb-10 {
  margin-bottom: 10px;
}
.mb-20 {
  margin-bottom: 20px;
}
.p-20 {
  padding: 20px;
}
.gap-15 {
  gap: 15px;
}

/* Display */
.hidden {
  display: none;
}
.visible {
  display: block;
}
.flex {
  display: flex;
}
.grid {
  display: grid;
}

/* Text Alignment */
.text-center {
  text-align: center;
}
.text-right {
  text-align: right;
}
.text-left {
  text-align: left;
}

/* Font Weight */
.bold {
  font-weight: bold;
}
.semi-bold {
  font-weight: 600;
}
.light {
  font-weight: 300;
}

/* Colors */
.text-gold {
  color: #d4af37;
}
.text-dark {
  color: #2c3e50;
}
.bg-dark {
  background-color: #2c3e50;
}
.bg-light {
  background-color: #f9f9f9;
}
```
