# Workshop Build Session Ideas

Quick web apps for the final hour build session using NextJS + SQLite.

## Team Lunch Orders

**Concept**: Office team coordinates daily lunch orders from local restaurants.

**Core Features**:
- Add restaurants with menu items and prices
- Users place daily lunch orders
- View order summaries and totals by day/person
- Track who owes money and payment status

**Database Schema**:
```sql
restaurants (id, name, phone, delivery_fee)
menu_items (id, restaurant_id, name, price, description)
lunch_orders (id, user_name, date, restaurant_id, total_amount, status)
order_items (id, order_id, menu_item_id, quantity, notes)
```

**Key Interactions**:
- Browse restaurants and menus
- Add items to daily order
- View today's group order summary
- Mark orders as paid/delivered

---

## Mini Library System

**Concept**: Simple library for office book sharing and lending.

**Core Features**:
- Add books to shared collection
- Check books out to team members
- Track due dates and returns
- Search books by title/author

**Database Schema**:
```sql
books (id, title, author, isbn, condition, date_added)
checkouts (id, book_id, user_id, checkout_date, due_date, return_date)
users (id, name, email)
```

**Key Interactions**:
- Browse available books
- Check out books (sets due date)
- Return books (updates status)
- View personal checkout history
- Search/filter book collection

---
