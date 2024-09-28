import json
import pandas as pd
from sklearn.model_selection import train_test_split
from sklearn.ensemble import RandomForestClassifier
from sklearn.linear_model import LogisticRegression
from sklearn.metrics import accuracy_score

# Функция для загрузки данных и подготовки признаков и целевой переменной
def load_and_prepare_data(dataset_path, target_column):
    # Загрузка данных из CSV
    data = pd.read_csv(dataset_path)
    
    # Проверяем наличие целевой колонки
    if target_column not in data.columns:
        raise ValueError(f"Target column '{target_column}' not found in dataset.")
    
    # Разделение данных на признаки и целевую переменную
    X = data.drop(columns=[target_column])
    y = data[target_column]
    
    return X, y

# Функция для выбора модели
def select_model(model_type, model_params):
    if model_type == "RandomForest":
        return RandomForestClassifier(**model_params)
    elif model_type == "LogisticRegression":
        return LogisticRegression(**model_params)
    else:
        raise ValueError(f"Unsupported model type: {model_type}")

# Основная функция для тренировки модели
def train_model(request_json):
    # Десериализация JSON параметров
    model_request = json.loads(request_json)
    
    # Получение параметров из JSON
    dataset_path = model_request['Dataset']
    model_type = model_request['ModelType']
    model_params = model_request['ModelParams']
    target_column = model_request['TargetColumn']  # Целевая колонка
    
    # Загрузка данных и подготовка
    X, y = load_and_prepare_data(dataset_path, target_column)
    
    # Разделение данных на тренировочный и тестовый наборы
    X_train, X_test, y_train, y_test = train_test_split(X, y, test_size=0.2, random_state=42)
    
    # Выбор модели
    model = select_model(model_type, model_params)
    
    # Тренировка модели
    model.fit(X_train, y_train)
    
    # Предсказание на тестовом наборе
    y_pred = model.predict(X_test)
    
    # Оценка модели
    accuracy = accuracy_score(y_test, y_pred)
    
    # Возвращаем результат в виде JSON
    result = {
        "accuracy": accuracy,
        "model_type": model_type,
        "model_params": model_params
    }
    
    return json.dumps(result)

# Пример вызова скрипта (в реальной среде этот код заменяется вызовом из веб-приложения)
if __name__ == "__main__":
    # Пример данных
    example_json = """
    {
        "Dataset": "path/to/dataset.csv",
        "ModelType": "RandomForest",
        "ModelParams": {"n_estimators": 100, "max_depth": 5},
        "TargetColumn": "target"
    }
    """
    
    result = train_model(example_json)
    print(result)
