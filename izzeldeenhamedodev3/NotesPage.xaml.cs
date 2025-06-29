using System.Collections.ObjectModel;
using System;
using System.Linq;
using Firebase.Database;
using Firebase.Database.Query;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.PlatformConfiguration.iOSSpecific;
using System.Collections.ObjectModel;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using Firebase.Database;
using Firebase.Database.Query;
using Microsoft.Maui.Controls;

namespace izzeldeenhamedodev3
{
 
        public partial class NotesPage : ContentPage
        {
            private readonly FirebaseClient _firebaseClient;
            private string _noteKey;
            private ObservableCollection<NoteData> _notes;

            public NotesPage()
            {
                InitializeComponent();
                _firebaseClient = new FirebaseClient("https://amrfirbase-default-rtdb.firebaseio.com/");
                _notes = new ObservableCollection<NoteData>();
                notesListView.ItemsSource = _notes;
                LoadNotes();
            }

            private async void LoadNotes()
            {
                var notes = await _firebaseClient
                    .Child("Notes")
                    .OnceAsync<NoteData>();

                _notes.Clear();
                foreach (var note in notes)
                {
                    var noteData = note.Object;
                    noteData.Key = note.Key;
                    _notes.Add(noteData);
                }
            }

            private async void OnSaveClicked(object sender, EventArgs e)
            {
                var date = datePicker.Date;
                var time = timePicker.Time;
                var note = noteEditor.Text;

                var noteData = new NoteData
                {
                    Date = date,
                    Time = time,
                    Note = note
                };

                var result = await _firebaseClient
                    .Child("Notes")
                    .PostAsync(noteData);

                noteData.Key = result.Key;
                _notes.Add(noteData);
            }

            private async void OnEditClicked(object sender, EventArgs e)
            {
                if (string.IsNullOrEmpty(_noteKey))
                {
                    await DisplayAlert("Error", "No note selected for editing", "OK");
                    return;
                }

                var date = datePicker.Date;
                var time = timePicker.Time;
                var note = noteEditor.Text;

                var noteData = new NoteData
                {
                    Date = date,
                    Time = time,
                    Note = note
                };

                await _firebaseClient
                    .Child("Notes")
                    .Child(_noteKey)
                    .PutAsync(noteData);

                var selectedNote = _notes.FirstOrDefault(n => n.Key == _noteKey);
                if (selectedNote != null)
                {
                    selectedNote.Date = date;
                    selectedNote.Time = time;
                    selectedNote.Note = note;
                }
            }

            private async void OnDeleteClicked(object sender, EventArgs e)
            {
                if (string.IsNullOrEmpty(_noteKey))
                {
                    await DisplayAlert("Error", "No note selected for deletion", "OK");
                    return;
                }

                await _firebaseClient
                    .Child("Notes")
                    .Child(_noteKey)
                    .DeleteAsync();

                var selectedNote = _notes.FirstOrDefault(n => n.Key == _noteKey);
                if (selectedNote != null)
                {
                    _notes.Remove(selectedNote);
                }

                _noteKey = null;
            }

            private void OnNoteSelected(object sender, SelectedItemChangedEventArgs e)
            {
                var selectedNote = e.SelectedItem as NoteData;
                if (selectedNote != null)
                {
                    datePicker.Date = selectedNote.Date;
                    timePicker.Time = selectedNote.Time;
                    noteEditor.Text = selectedNote.Note;
                    _noteKey = selectedNote.Key;
                }
            }
        }

        public class NoteData
        {
            public string Key { get; set; }
            public DateTime Date { get; set; }
            public TimeSpan Time { get; set; }
            public string Note { get; set; }
        }
}